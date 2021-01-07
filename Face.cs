using System.Collections;
using System;
        public class Face
        {
            private ArrayList face= new ArrayList();
            private int count=0;
            public void Add(Shape s)
            {
                face.Add(s);
                count++;
                Console.WriteLine("Added feature to face: {0}" + Environment.NewLine, s);
            }
            public void Remove()
            {
                face.RemoveAt(count);
                Console.WriteLine("Removed feature from face: ");
            }

            public Face(){
                 Console.WriteLine("Face Created - use commands to add Features to	the Face"); Console.WriteLine();
            }

            public override string ToString()
            {
                String str = "Face (" + face.Count + " feature(s)): " + Environment.NewLine + Environment.NewLine;
                foreach (Shape s in face)
                {
                    str += "   > " + s.MakeString() + Environment.NewLine;
                }
                return str;
            }
            public ArrayList GetState()
            {
                return this.face;
            }
        
            public void SetState(Memento savedOne)
            {
                face.Clear();
                ArrayList newar =savedOne.GetState();
                foreach(var item in newar){
                    face.Add(item);
                }
            }

            public int getCount(){
                return face.Count;
            }

        }