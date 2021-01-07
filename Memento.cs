using System.Collections;
using System;
public class Memento
{
   private ArrayList state=new ArrayList();
 
    public ArrayList GetState()
    {
        return this.state;
    }
 
    public void SetState(ArrayList state)
    {
        this.state=state;
    }

    public Memento CreateMemento()
    {
        Memento shapeMemento = new Memento();
        shapeMemento.SetState(this.state);
        return shapeMemento;
    }
        
    public void SetMemento(ArrayList ar)
    {
        state.Clear();
        foreach(var item in ar){
            state.Add(item);
        }
    }
}