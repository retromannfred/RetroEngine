namespace RetroEngine.UnitTest.TestData.Components
{
    internal struct TagComponent
    {
        public string Tag { get; set; } = string.Empty;

        public TagComponent() { }

        public TagComponent(string tag)
        {
            Tag = tag;
        }
    }
}
