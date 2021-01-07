public class RightEyeDecorator : ShapeDecorator
{
    private string Style="fill.black";
    private string CX="156";
    private string CY="104";
    private string R="12";
    public RightEyeDecorator(Shape shape) : base(shape){}
    public override string MakeString()
    {
        string [] ar = shape.MakeString().Split("(");
        string shapeorig= ar[0];
        return shapeorig +RightEye();
    }
    private string RightEye()
    {
        return "(class: " + "right-eye" +", cx: " + CX + ", cy: " + CY + ", r: " + R+ ", style: " + Style +")";
    }
    public override void setStyle(string style){
        this.Style=style; 
    }
    public override void setDim(string dimensions){
        string [] splitter = dimensions.Split(",");
        this.CX=splitter[0];
        this.CY=splitter[1];
        this.R=splitter[2];
    }
}