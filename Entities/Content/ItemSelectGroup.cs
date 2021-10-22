namespace Api.Entities.Content
{
    public class ItemSelectGroup: Item
    {
        public int HowManySelectsAreValid { get; set; }
        public override ItemType Type => ItemType.SelectGroup;
    }
}
