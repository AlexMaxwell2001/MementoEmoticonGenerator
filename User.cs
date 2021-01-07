using System;
using System.Collections.Generic;
public class User
{
    private Stack<Memento> undo;
    private Stack<Memento> redo;
    public int UndoCount { get => undo.Count; }
    public int RedoCount { get => undo.Count; }
    public User()
    {
        Reset();
    }
    public void Reset()
    {
        undo = new Stack<Memento>();
        redo = new Stack<Memento>();
    }

    public void Action(Shape s, Face current, string tell)
    {
        Memento m=new Memento();
        m.SetMemento(current.GetState());
        undo.Push(m); 
        if(tell == null) current.Add(s);
        redo.Clear(); 
    }
    public void Undo(Face face)
    {
        if (undo.Count > 0)
        {
            Memento old=new Memento();
            old.SetMemento(face.GetState());
            Memento c = undo.Pop();face.SetState(c);redo.Push(old);
            Console.WriteLine("Undoing operation!"); 
        }else {
            Console.WriteLine("No operation to undo!"); 
        }

    }
     public void Redo(Face face)
    {
        if (redo.Count > 0)
        {
            Console.WriteLine("Redoing operation!"); 
            Memento c = redo.Pop(); face.SetState(c); undo.Push(c);
        }else{
            Console.WriteLine("No operation to be redone!"); 
        }

    }
    public Stack<Memento> getUndo(){
        return this.undo;
    }
    public void setUndo(Stack<Memento> newundo){
        this.undo=newundo;
    }
}