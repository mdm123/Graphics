using UnityEngine.Graphing;

namespace UnityEngine.MaterialGraph
{
    [Title("Input/Vector/Vector 1")]
    public class Vector1Node : PropertyNode, IGeneratesBodyCode
    {
        [SerializeField]
        private float m_Value;

		[SerializeField]
		private FloatPropertyChunk.FloatType m_floatType;

		[SerializeField]
		private Vector3 m_rangeValues = new Vector3(0f, 1f, 2f);

        public const int OutputSlotId = 0;
        private const string kOutputSlotName = "Value";

        public Vector1Node()
        {
            name = "Vector1";
            UpdateNodeAfterDeserialization();
        }

        public sealed override void UpdateNodeAfterDeserialization()
        {
            AddSlot(new MaterialSlot(OutputSlotId, kOutputSlotName, kOutputSlotName, SlotType.Output, SlotValueType.Vector1, Vector4.zero));
            RemoveSlotsNameNotMatching(new[] { OutputSlotId });
        }

        public override PropertyType propertyType
        {
            get { return PropertyType.Float; }
        }

        public float value
        {
            get { return m_Value; }
            set
            {
                if (m_Value == value)
                    return;

                m_Value = value;

                if (onModified != null)
                    onModified(this, ModificationScope.Node);
            }
        }

		public FloatPropertyChunk.FloatType floatType
		{
			get { return m_floatType; }
			set
			{
				if (m_floatType == value)
					return;

				m_floatType = value;

				if (onModified != null)
					onModified(this, ModificationScope.Node);
			}
		}

		public Vector3 rangeValues
		{
			get { return m_rangeValues; }
			set
			{
				if (m_rangeValues == value)
					return;

				m_rangeValues = value;

				if (onModified != null)
					onModified(this, ModificationScope.Node);
			}
		}

        public override void GeneratePropertyBlock(PropertyGenerator visitor, GenerationMode generationMode)
        {
			if (exposedState == ExposedState.Exposed) {
				switch(m_floatType){
				case FloatPropertyChunk.FloatType.Float:
					visitor.AddShaderProperty (new FloatPropertyChunk (propertyName, description, m_Value, PropertyChunk.HideState.Visible));
					break;
				case FloatPropertyChunk.FloatType.Toggle:
					visitor.AddShaderProperty (new FloatPropertyChunk (propertyName, description, m_Value, m_floatType, PropertyChunk.HideState.Visible));
					break;
				case FloatPropertyChunk.FloatType.Range:
					visitor.AddShaderProperty (new FloatPropertyChunk (propertyName, description, m_Value, m_floatType, m_rangeValues, PropertyChunk.HideState.Visible));
					break;
				case FloatPropertyChunk.FloatType.PowerSlider:
					visitor.AddShaderProperty (new FloatPropertyChunk (propertyName, description, m_Value, m_floatType, m_rangeValues, PropertyChunk.HideState.Visible));
					break;
				}
			}
        }

        public override void GeneratePropertyUsages(ShaderGenerator visitor, GenerationMode generationMode)
        {
            if (exposedState == ExposedState.Exposed || generationMode.IsPreview())
                visitor.AddShaderChunk(precision + " " + propertyName + ";", true);
        }

        public void GenerateNodeCode(ShaderGenerator visitor, GenerationMode generationMode)
        {
            if (exposedState == ExposedState.Exposed || generationMode.IsPreview())
                return;

            visitor.AddShaderChunk(precision + " " + propertyName + " = " + m_Value + ";", true);
        }

        public override PreviewProperty GetPreviewProperty()
        {
            return new PreviewProperty
                   {
                       m_Name = propertyName,
                       m_PropType = PropertyType.Float,
                       m_Float = m_Value
                   };
        }
    }
}
