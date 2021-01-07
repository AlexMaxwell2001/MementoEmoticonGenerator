public class LeftEyeDecorator : ShapeDecorator
{
    private string Style="fill.black";
    private string CX="100";
    private string CY="104";
    private string R="12";
    public LeftEyeDecorator(Shape shape) : base(shape)
    {
    }
    public override string MakeString()
    {
        string [] ar = shape.MakeString().Split("(");
        string shapeorig= ar[0];
        return shapeorig +LeftEye();
    }
    private string LeftEye()
    {
        return "(class: " + "left-eye" +", cx: " + CX + ", cy: " + CY + ", r: " + R + ", style: "+ Style+")";
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