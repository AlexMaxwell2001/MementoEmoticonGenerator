public abstract class ShapeDecorator : Shape
{
    protected Shape shape;
    public ShapeDecorator(Shape shape)
    {
        this.shape = shape;
    }
    public virtual string MakeString()
    {
        return shape.MakeString();
    }
    public virtual void setStyle(string style){}
    public virtual void setDim(string dims){}
}