public class LeftBrowDecorator : ShapeDecorator
{
    private string Style="fill.none@stroke.#000@stroke-width.6px@stroke-linecap.round";
    private string D="M65,85 Q74,67 115,75";
    public LeftBrowDecorator(Shape shape) : base(shape)
    {
    }
    public override string MakeString()
    {
        string [] ar = shape.MakeString().Split("(");
        string shapeorig= ar[0];
        return shapeorig +LeftBrow();
    }
    private string LeftBrow()
    {
        return "(class: " + "left-brow"+ ", d: " + D + "  , style: " + Style + ")";
    }

    public override void setStyle(string style){
        int inde = Style.IndexOf("@");
        string newString=Style.Substring(inde);
        int inde2 = newString.IndexOf("@");
        string newString2=newString.Substring(inde);
        this.Style=style+newString2.Replace("0","");
    }

    public override void setDim(string dimensions){
        this.D=dimensions;
    }
}