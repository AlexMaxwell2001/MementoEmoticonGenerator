public class MouthDecorator : ShapeDecorator
{
    private string Style = "fill.none@stroke.#000@stroke-width.6px@stroke-linecap.round";
    private string D="M100,160 Q128,190 156,160";
    public MouthDecorator(Shape shape) : base(shape){}
    public override string MakeString()
    {
        string [] ar = shape.MakeString().Split("(");
        string shapeorig= ar[0];
        return shapeorig +Mouth();
    }
    private string Mouth()
    {
        return "(class: " + "mouth"+ ", d: " + D + "  , style: " + Style + ")";
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