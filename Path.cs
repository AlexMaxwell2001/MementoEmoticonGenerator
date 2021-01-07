// Path Shape class
public class Path : Shape
{
    public string D { get; private set; }
    public int Id { get; private set; }
    public string Style { get; private set; }
    public Path(string d,int id,string style)
    {
        D = d;Id=id;Style=style;
    }
    public Path()
    {
        D = "M20,230 Q40,205 50,230 T90,230";Id=3;Style="stroke.green";
    }

    public string MakeString()
    {
        return "Path (id: " + Id+ ", d: " + D + "  , style: " + Style + ")";
    }
    public void setStyle(string style){
        this.Style=style;
    }
    public void setDim(string direction){
        this.D=D;
    }
}