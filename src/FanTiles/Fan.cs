using KSerialization;
using STRINGS;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fans
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class Fan : KMonoBehaviour, ISim200ms, IEffectDescriptor
    {
        public static readonly Operational.Flag FanInFlag = new Operational.Flag("fanIn", Operational.Flag.Type.Requirement);
        public static readonly Operational.Flag FanOutFlag = new Operational.Flag("fanOut", Operational.Flag.Type.Requirement);

        [SerializeField]
        public ConduitType conduitType;
        [SerializeField]
        public float overpressureMass = 1f;

        [MyCmpGet]
        private Storage storage;
        [MyCmpReq]
        private Operational operational;
        [MyCmpGet]
        private KSelectable selectable;
        [MyCmpGet]
        private PrimaryElement exhaustPE;

        private const float OperationalUpdateInterval = 1f;
        private float elapsedTime;
        private bool pumpable;
        private bool ventable;

        private Guid obstructedStatusGuid;
        private Guid overPressureStatusGuid;
        private Guid noElementStatusGuid;

        private int inputCell = -1;
        private int outputCell = -1;


        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            Vector3 position = transform.GetPosition();
            Rotatable rotatable = GetComponent<Rotatable>();
            Vector3 rotatedInputOffset = Rotatable.GetRotatedOffset(new Vector3(0, -1), rotatable.GetOrientation());
            Vector3 rotatedOutputOffset = Rotatable.GetRotatedOffset(new Vector3(0, 1), rotatable.GetOrientation());
            inputCell = Grid.PosToCell(position + rotatedInputOffset);
            outputCell = Grid.PosToCell(position + rotatedOutputOffset);

            elapsedTime = 0.0f;
            pumpable = UpdatePumpOperational();
            ventable = UpdateVentOperational();
        }

        public Vent.State GetEndPointState()
        {
            Vent.State state = Vent.State.Ready;
            if (!IsValidOutputCell(outputCell))
            {
                state = !Grid.Solid[outputCell] ? Vent.State.OverPressure : Vent.State.Blocked;
            }
            return state;
        }

        public bool IsBlocked
        {
            get
            {
                return GetEndPointState() != Vent.State.Ready;
            }
        }

        private bool IsValidOutputCell(int output_cell)
        {
            bool flag = false;
            if (!Grid.Solid[output_cell])
            {
                flag = true;
                if (overpressureMass >= 0.0f)
                {
                    flag = Grid.Mass[output_cell] < (double)overpressureMass;
                }
            }
            return flag;
        }

        private bool UpdateVentOperational()
        {
            Vent.State outputState = GetEndPointState();
            bool obstructedFlag = outputState == Vent.State.Blocked;
            bool overPressureFlag = outputState == Vent.State.OverPressure;
            obstructedStatusGuid = selectable.ToggleStatusItem(conduitType != ConduitType.Gas ? Db.Get().BuildingStatusItems.LiquidVentObstructed : Db.Get().BuildingStatusItems.GasVentObstructed, obstructedStatusGuid, obstructedFlag, null);
            overPressureStatusGuid = selectable.ToggleStatusItem(conduitType != ConduitType.Gas ? Db.Get().BuildingStatusItems.LiquidVentOverPressure : Db.Get().BuildingStatusItems.GasVentOverPressure, overPressureStatusGuid, overPressureFlag, null);
            bool flag = !obstructedFlag && !overPressureFlag;
            operational.SetFlag(FanOutFlag, flag);
            return flag;
        }

        public List<Descriptor> GetDescriptors(BuildingDef def)
        {
            if (overpressureMass < 0.0f)
            {
                return new List<Descriptor>() { };
            }
            string formattedMass = GameUtil.GetFormattedMass(overpressureMass, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.UseThreshold, true, "{0:0.#}");
            return new List<Descriptor>()
            {
                new Descriptor(string.Format(UI.BUILDINGEFFECTS.OVER_PRESSURE_MASS, formattedMass), string.Format(UI.BUILDINGEFFECTS.TOOLTIPS.OVER_PRESSURE_MASS, formattedMass), Descriptor.DescriptorType.Effect, false)
            };
        }

        public void Sim200ms(float dt)
        {
            elapsedTime += dt;
            if (elapsedTime >= OperationalUpdateInterval)
            {
                pumpable = UpdatePumpOperational();
                ventable = UpdateVentOperational();
                elapsedTime = 0.0f;
            }
            // perform pumping
            DoFan();
            if (operational.IsOperational && pumpable && ventable)
                operational.SetActive(true, false);
            else
                operational.SetActive(false, false);
        }

        private bool IsPumpable(Element.State expected_state)
        {
            return Grid.Element[inputCell].IsState(expected_state);
        }

        private bool UpdatePumpOperational()
        {
            Element.State expected_state = Element.State.Vacuum;
            switch (conduitType)
            {
                case ConduitType.Gas:
                    expected_state = Element.State.Gas;
                    break;
                case ConduitType.Liquid:
                    expected_state = Element.State.Liquid;
                    break;
            }
            bool flag = IsPumpable(expected_state);
            noElementStatusGuid = selectable.ToggleStatusItem(expected_state != Element.State.Gas ? Db.Get().BuildingStatusItems.NoLiquidElementToPump : Db.Get().BuildingStatusItems.NoGasElementToPump, noElementStatusGuid, !flag, null);
            operational.SetFlag(FanInFlag, flag);
            return flag;
        }

        private void DoFan()
        {
            if (storage.items.Count == 0)
            {
                return;
            }
            if (Grid.Solid[outputCell])
            {
                return;
            }
            switch (conduitType)
            {
                case ConduitType.Gas:
                    EmitGas(outputCell);
                    break;
                case ConduitType.Liquid:
                    EmitLiquid(outputCell);
                    break;
            }
        }

        private void CalculateDiseaseTransfer(
          PrimaryElement item1,
          PrimaryElement item2,
          float transfer_rate,
          out int disease_to_item1,
          out int disease_to_item2)
        {
            disease_to_item1 = (int)(item2.DiseaseCount * transfer_rate);
            disease_to_item2 = (int)(item1.DiseaseCount * transfer_rate);
        }

        private bool EmitCommon(int cell, PrimaryElement primary_element, EmitDelegate emit)
        {
            if (primary_element.Mass <= 0.0)
                return false;
            int disease_to_item1;
            int disease_to_item2;
            CalculateDiseaseTransfer(exhaustPE, primary_element, 0.05f, out disease_to_item1, out disease_to_item2);
            primary_element.ModifyDiseaseCount(-disease_to_item1, "Exhaust transfer");
            primary_element.AddDisease(exhaustPE.DiseaseIdx, disease_to_item2, "Exhaust transfer");
            exhaustPE.ModifyDiseaseCount(-disease_to_item2, "Exhaust transfer");
            exhaustPE.AddDisease(primary_element.DiseaseIdx, disease_to_item1, "Exhaust transfer");
            emit(cell, primary_element);
            primary_element.KeepZeroMassObject = true;
            primary_element.Mass = 0.0f;
            primary_element.ModifyDiseaseCount(int.MinValue, "Exhaust.SimUpdate");
            return true;
        }

        private void EmitLiquid(int cell)
        {
            int cell1 = Grid.CellBelow(cell);
            EmitDelegate emit = !Grid.IsValidCell(cell1) || Grid.Solid[cell1] ? emit_element : emit_particle;
            foreach (GameObject gameObject in storage.items)
            {
                PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
                if (component.Element.IsLiquid && EmitCommon(cell, component, emit))
                    break;
            }
        }

        private void EmitGas(int cell)
        {
            foreach (GameObject gameObject in storage.items)
            {
                PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
                if (component.Element.IsGas && EmitCommon(cell, component, emit_element))
                    break;
            }
        }

        private delegate void EmitDelegate(int cell, PrimaryElement primary_element);
        private static EmitDelegate emit_element = (cell, primary_element) => SimMessages.AddRemoveSubstance(cell, primary_element.ElementID, CellEventLogger.Instance.ExhaustSimUpdate, primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, -1);
        private static EmitDelegate emit_particle = (cell, primary_element) => FallingWater.instance.AddParticle(cell, (byte)ElementLoader.elements.IndexOf(primary_element.Element), primary_element.Mass, primary_element.Temperature, primary_element.DiseaseIdx, primary_element.DiseaseCount, true, false, true, false);
    }
}
