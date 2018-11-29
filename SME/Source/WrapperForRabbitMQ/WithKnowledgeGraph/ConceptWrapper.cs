namespace SME.Models
{
    public class ConceptWrapper
    {
        public ConceptWrapper(Concept c)
        {
            Name = c.Name;
        }
        public string Name { get; set; }
    }
}