// Circle Shape class
using System;
public class Circle : Shape
{
    public int CX { get; private set; }
    public int CY { get; private set; }
    public int R { get; private set; }
    public string Id { get; private set; }
    public string Style{ get; private set; }
    public Circle(int cx, int cy, int r,string id,string style)
    {
        CX = cx; CY = cy; R = r;Id=id;Style=style;
    }
    public Circle()
    {
        CX = 128; CY = 128; R = 120;Id="face";Style="fill.gold@stroke-width.4@stroke.#000000";
    }
    public string MakeString()
    {
        return "Circle (class: " + Id +", cx: " + CX + ", cy: " + CY + ", r: " + R + ", style: " + Style +")";
    }
    public void setStyle(string style){
        this.Style=style;
    }
    public void setDim(string dimensions){
        this.CX=Int16.Parse(dimensions);
    }
}