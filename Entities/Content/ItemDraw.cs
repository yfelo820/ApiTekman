namespace Api.Entities.Content
{
    public class ItemDraw: Item
    {
        public string LineColor { get; set; }
        public int LineWidth { get; set; }
        public override ItemType Type => ItemType.Draw;
    }
}
