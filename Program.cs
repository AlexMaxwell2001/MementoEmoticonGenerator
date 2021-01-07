using System;
using System.IO;
using System.Collections;
using System.Text.RegularExpressions;
//OS:Windows , IDE:VS Code 1.51.0

/*CLI User Interface has been implemented as per the specification i.e. these commands below can be used as it was laid out in the PDF
     add	 {left-eye|right-eye|left-brow|right-brow|mouth}	
	 remove	 {left-eye|right-eye|left-brow|right-brow|mouth}	
	 move	 {left-eye|right-eye|left-brow|right-brow|mouth} {up|down|left|right}	
	 reset	 {left-eye|right-eye|left-brow|right-brow|mouth}	
	 style	 {left-eye|right-eye|left-brow|right-brow|mouth} {1|2}	
	 save	 {filename}	
	 draw	
	 undo	
	 redo	
	 help	 	
	 quit
  
  Design patterns used throughout this application:
  Apart from the Memento and Command design patterns being implemented for undo/redo functionality, I used the Decorator pattern to build the structure of my application.
  Design patterns are used to have common solutions to common problems. I felt like this way the optimal pattern to use given the problem statement and what I have associated with the pattern in past experience.
  The idea of using this pattern in my application, I wanted to build upon the pre-existing objects. This suits the specification as we were required to create features on a face with svg shapes. Building on those
  svg shapes I was able to implement easily the shapes I wanted in an OOP approach.


  Comparision between the Memento and Command on undo/redo functionality in both my solutions:
  The Command Design pattern is much easier to develop than the Memento Design pattern, the Memento pattern takes alot of understanding pre-implementation compared to the Command pattern.
  I feel the Memento pattern is much better in this specific application,Users in the application may want to jump to different internal states and with command pattern you can't do that so Memento pattern ismore feasible for this reason.
  Memento pattern there is a lot less code and it's more neater even though but it's much more complex.The Memento pattern for this program and all programs, generally would scale much better,
  due to less code, less object creation (Command makes an object each time a shape is added), and with the Command pattern it's less dynamic then the Memento pattern for the undo-redo 
  functionality as it can only more undo one at a time to get where it wants to be but with Memento there is the ability to choose the state immediately. Also on scalability for this application, their is less worry about error handling with breaking the history
  the memento pattern. This is a vital requirement as if history was broken in the Command design pattern it would be much more harder to debug. Also due to the restriction of the features to only 5 the Memento is in it's optimal enviroment. 
  Avoiding Large memory history would can be a downfall to the pattern.
  */

    class Program{
        public static Face face = new Face();
        public static User user = new User();
        public static void Main()
        {
            string svg="</svg>";
            Console.WriteLine("Use Help to see Commands associated with the application!"+ Environment.NewLine);
            face.Add(new Circle());
            DrawingDisplay();
            while(true){
                string input=Console.ReadLine();
                if(input.ToUpper()=="QUIT")break;
                string [] ar= input.Split(" ");
                string first = ar[0];
                if(first.ToUpper().Equals("ADD")){
                    AddingFacialFeatures(ar[1].Replace("{","").Replace("}",""));
                }else if(first.ToUpper().Equals("UNDO")){
                    user.Undo(face);
                }else if(first.ToUpper().Equals("STYLE")){
                    StylingFeatures(ar[1].Replace("{","").Replace("}",""),ar[2].Replace("{","").Replace("}",""));
                }else if(first.ToUpper().Equals("REDO")){
                    user.Redo(face);
                }else if(first.ToUpper().Equals("REMOVE")){
                    RemoveFacialFeatures(ar[1].Replace("{","").Replace("}",""));
                }else if(first.ToUpper().Equals("MOVE")){
                    MoveFacialFeatures(ar[1].Replace("{","").Replace("}",""),ar[2].Replace("{","").Replace("}",""));
                }else if(first.ToUpper().Equals("HELP")){
                    commandPrintout();
                }else if(first.ToUpper().Equals("DRAW")){
                    DrawingDisplay();
                }else if(first.ToUpper().Equals("SAVE")){
                    if(face.ToString().Length>0){
                        string nothing="";string filePath= ar[1].Replace("{","").Replace("}","");
                        File.WriteAllText(filePath,nothing);
                        string var= face.ToString();
                        File.WriteAllText(filePath,var);
                        string [] tempar= File.ReadAllLines(filePath);
                        string canvasstring = "<svg width="+@"""" +250+@""""+" height="+ @""""+250+@""""+ " version="+ @""""+1.1+@""""+ " xmlns="+@""""+ "http:"+ "//www.w3.org/2000/svg"+ @""""+">"+ Environment.NewLine +svg;
                        File.WriteAllText(filePath,canvasstring);Save(tempar,filePath);
                    }
                }else if(first.ToUpper().Equals("RESET")){
                   ResetFacialFeatures();
                }else{
                    Console.WriteLine("The command " + first+ " doesn't exist, you can use the Help command to see available commands");
                }
            }
            Console.WriteLine("Goodbye!");
        }
        public static void DrawingDisplay(){
            Console.WriteLine("Face (" + face.getCount() + " feature(s)): " + Environment.NewLine);
            string nothing="";string filePath= "donotuse.svg";File.WriteAllText(filePath,nothing);
            string var= face.ToString();File.WriteAllText(filePath,var);
            string [] tempar= File.ReadAllLines(filePath);
            File.Delete(filePath);
            Console.WriteLine(drawSVG(tempar));
        }
        public static void AddingFacialFeatures(string second){
            Console.WriteLine("Adding feature(s) to the face");
            if(second.Contains("|")){
                string [] temporary= second.Split("|");
                string [] temporary2= face.ToString().Split(">");
                int count=0;
                foreach(var i in temporary){
                    temporary2= face.ToString().Split(">");
                    count=0;
                    foreach(var j in temporary2){ count++;}
                    if(count >= 7) Console.WriteLine("Sorry can't add any more features. 5 is the max!(excluding the face)");
                    else if(i.Equals("left-eye")||i.Equals("Left-Eye")){
                        user.Action(new LeftEyeDecorator(new Circle()), face, null);
                    }else if(i.Equals("right-eye")||i.Equals("Right-Eye")){
                        user.Action(new RightEyeDecorator(new Circle()), face, null);
                    }else if(i.Equals("mouth")||i.Equals("Mouth")){
                        user.Action(new MouthDecorator(new Path()), face, null);
                    }else if(i.Equals("left-brow")||i.Equals("Left-Brow")){
                        user.Action(new LeftBrowDecorator(new Path()), face, null);
                    }else if(i.Equals("right-brow")||i.Equals("Right-Brow")){
                        user.Action(new RightBrowDecorator(new Path()), face, null);
                    }else{
                        Console.WriteLine("The shape " + i + " doesn't exist!");
                    }
                    count++;
                }
            }else{
                string [] temporary= face.ToString().Split(">");
                int count=0;
                foreach(var i in temporary){ count++;}
                if(count >= 7) Console.WriteLine("Sorry can't add any more features. 5 is the max!(excluding the face)");
                else if(second.Equals("left-eye")||second.Equals("Left-Eye")){
                    user.Action(new LeftEyeDecorator(new Circle()), face, null);
                }else if(second.Equals("right-eye")||second.Equals("Right-Eye")){
                    user.Action(new RightEyeDecorator(new Circle()), face,null);
                }else if(second.Equals("mouth")||second.Equals("Mouth")){
                    user.Action(new MouthDecorator(new Path()), face,null);
                }else if(second.Equals("left-brow")||second.Equals("Left-Brow")){
                    user.Action(new LeftBrowDecorator(new Path()), face,null);
                }else if(second.Equals("right-brow")||second.Equals("Right-Brow")){
                    user.Action(new RightBrowDecorator(new Path()), face, null);
                }else{
                    Console.WriteLine("The shape " + second + " doesn't exist!");
                }
            }
        }
        public static string RemoveSpaces(string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }
        public static void Save(string [] tempar,string filePath){
            for(int i=2;i<tempar.Length;i++){
                if(tempar[i].Contains(":")){
                    string [] temp= tempar[i].Split(" (");string [] temp2= tempar[i].Split(":");
                    string shape=temp[0].Replace(">","");string shapes=RemoveSpaces(shape);
                    if(shapes.Equals(" Circle")){
                        string [] ar1=temp2[1].Split(",");string id=ar1[0].Replace(" ","");
                        string [] ar2=temp2[2].Split(",");string cx=ar2[0].Replace(" ","");
                        string [] ar3=temp2[3].Split(",");string cy=ar3[0].Replace(" ","");
                        string [] ar4=temp2[4].Split(",");string r=ar4[0].Replace(" ","");
                        string style=temp2[5].Replace(".",":").Replace(")","").Replace(" ","").Replace("@",";");
                        addShape(cx,cy,r,null,shapes,id,style,filePath);
                    }else if(shapes.Equals(" Path")){
                        string [] ar1=temp2[1].Split(",");string id=ar1[0].Replace(" ","");
                        string d=temp2[2].Replace("  , style","");
                        string style=temp2[3].Replace(".",":").Replace(")","").Replace(" ","").Replace("@",";");
                        addShape(d,null,null,null,shapes,id,style,filePath);
                    }
                }     
            }
        }
        public static void MoveFacialFeatures(string s1,string s2){
            if(s1.Contains("|") && s2.Contains("|")){
                string [] breaker1= s1.Split("|");
                string [] breaker2= s2.Split("|");
                int min= Math.Min(breaker1.Length, breaker2.Length);
                for(int m=0;m<min;m++)
                {
                    s1=breaker1[m];s2=breaker2[m];
                    ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                    Shape door=null;
                    foreach(Shape ik in nn){
                        if(ik.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                            door=ik;
                        }else{
                            temm.Add(ik);
                        }
                    }
                    string finale="", info=door.MakeString();
                    int change=15;
                    if(door.MakeString().Contains("Circle")){
                        string [] temp= info.Split(" (");string [] temp2= info.Split(":");
                        string [] ar2=temp2[2].Split(",");int cx=Int16.Parse(ar2[0].Replace(" ",""));
                        string [] ar3=temp2[3].Split(",");int cy=Int16.Parse(ar3[0].Replace(" ",""));
                        string [] ar4=temp2[4].Split(",");int r=Int16.Parse(ar4[0].Replace(" ",""));
                        if(s2.Equals("up")){
                            finale+=cx.ToString()+","+(cy-change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            finale+=cx.ToString()+","+(cy+change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            finale+=(cx-change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            finale+=(cx+change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else if(door.MakeString().Contains("Path")){
                        string []splitty=info.Split("d: ");
                        string [] splitty2=splitty[1].Split("  , style");
                        string d=splitty2[0];
                        if(s2.Equals("up")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])-change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])+change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)-change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)+change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else{
                        Console.WriteLine("This feature can't be moved!");
                    }
                    
                    temm.Add(door);
                    Memento this1=new Memento();
                    this1.SetState(temm);
                    face.SetState(this1);
                }
                }else if(s1.Contains("|") ){
                    string [] breaker = s1.Split("|");
                    s1=breaker[0];
                    ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                    Shape door=null;
                    foreach(Shape i in nn){
                        if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                            door=i;
                        }else{
                            temm.Add(i);
                        }
                    }
                    string finale="", info=door.MakeString();
                    int change=15;
                    if(door.MakeString().Contains("Circle")){
                        string [] temp= info.Split(" (");string [] temp2= info.Split(":");
                        string [] ar2=temp2[2].Split(",");int cx=Int16.Parse(ar2[0].Replace(" ",""));
                        string [] ar3=temp2[3].Split(",");int cy=Int16.Parse(ar3[0].Replace(" ",""));
                        string [] ar4=temp2[4].Split(",");int r=Int16.Parse(ar4[0].Replace(" ",""));
                        if(s2.Equals("up")){
                            finale+=cx.ToString()+","+(cy-change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            finale+=cx.ToString()+","+(cy+change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            finale+=(cx-change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            finale+=(cx+change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else if(door.MakeString().Contains("Path")){
                        string []splitty=info.Split("d: ");
                        string [] splitty2=splitty[1].Split("  , style");
                        string d=splitty2[0];
                        if(s2.Equals("up")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])-change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])+change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)-change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)+change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else{
                        Console.WriteLine("This feature can't be moved!");
                    }
                    
                    temm.Add(door);
                    Memento this1=new Memento();
                    this1.SetState(temm);
                    face.SetState(this1);
                }else if(s2.Contains("|")){
                    string [] breaker=s2.Split("|");
                    s2=breaker[0]; 
                    Console.WriteLine("problem"+ s2);
                    ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                    Shape door=null;
                    foreach(Shape i in nn){
                        if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                            door=i;
                        }else{
                            temm.Add(i);
                        }
                    }
                    string finale="", info=door.MakeString();
                    int change=15;
                    if(door.MakeString().Contains("Circle")){
                        string [] temp= info.Split(" (");string [] temp2= info.Split(":");
                        string [] ar2=temp2[2].Split(",");int cx=Int16.Parse(ar2[0].Replace(" ",""));
                        string [] ar3=temp2[3].Split(",");int cy=Int16.Parse(ar3[0].Replace(" ",""));
                        string [] ar4=temp2[4].Split(",");int r=Int16.Parse(ar4[0].Replace(" ",""));
                        if(s2.Equals("up")){
                            finale+=cx.ToString()+","+(cy-change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            finale+=cx.ToString()+","+(cy+change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            finale+=(cx-change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            finale+=(cx+change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else if(door.MakeString().Contains("Path")){
                        string []splitty=info.Split("d: ");
                        string [] splitty2=splitty[1].Split("  , style");
                        string d=splitty2[0];
                        if(s2.Equals("up")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])-change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])+change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)-change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)+change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else{
                        Console.WriteLine("This feature can't be moved!");
                    }
                    
                    temm.Add(door);
                    Memento this1=new Memento();
                    this1.SetState(temm);
                    face.SetState(this1); 
                }else{
                    ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                    Shape door=null;
                    foreach(Shape i in nn){
                        if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                            door=i;
                        }else{
                            temm.Add(i);
                        }
                    }
                    string finale="", info=door.MakeString();
                    int change=15;
                    if(door.MakeString().Contains("Circle")){
                        string [] temp= info.Split(" (");string [] temp2= info.Split(":");
                        string [] ar2=temp2[2].Split(",");int cx=Int16.Parse(ar2[0].Replace(" ",""));
                        string [] ar3=temp2[3].Split(",");int cy=Int16.Parse(ar3[0].Replace(" ",""));
                        string [] ar4=temp2[4].Split(",");int r=Int16.Parse(ar4[0].Replace(" ",""));
                        if(s2.Equals("up")){
                            finale+=cx.ToString()+","+(cy-change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            finale+=cx.ToString()+","+(cy+change).ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            finale+=(cx-change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            finale+=(cx+change).ToString()+","+cy.ToString()+","+r.ToString();
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else if(door.MakeString().Contains("Path")){
                        string []splitty=info.Split("d: ");
                        string [] splitty2=splitty[1].Split("  , style");
                        string d=splitty2[0];
                        if(s2.Equals("up")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])-change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("down")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string add=arr1[0];int changing=Int16.Parse(arr1[1])+change;
                                if(i ==arr.Length-1) finale+=add+","+ changing.ToString();
                                else finale+=add+","+ changing.ToString()+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("left")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)-change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else if(s2.Equals("right")){
                            string [] arr = d.Split(" ");
                            for(int i=0;i<arr.Length;i++){
                                string [] arr1 = arr[i].Split(",");
                                string notletters = Regex.Replace(arr1[0], "[^0-9.]", "");
                                string letters = Regex.Replace(arr1[0], "[0-9.]", "");
                                int check= Int16.Parse(notletters)+change;
                                string add=arr1[1];string changing= letters+check.ToString();
                                if(arr.Length-1 == i)finale+=changing+","+ add;
                                else finale+=changing+","+ add+" ";
                            }
                            door.setDim(finale);
                        }else{
                            Console.WriteLine("This is not a direction to move in");
                        }
                    }else{
                        Console.WriteLine("This feature can't be moved!");
                    }
                    
                    temm.Add(door);
                    Memento this1=new Memento();
                    this1.SetState(temm);
                    face.SetState(this1);
                }
        }
        public static void StylingFeatures(string s1,string s2){
            if(s1.Contains("|") && s2.Contains("|")){
                string [] breaker1= s1.Split("|");
                string [] breaker2= s2.Split("|");
                int min= Math.Min(breaker1.Length, breaker2.Length);
                for(int i=0;i<min;i++)
                {
                    s1=breaker1[i];s2=breaker2[i];
                    ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                    Shape key=null;
                    foreach(Shape k in nn){
                        if(k.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                            key=k;
                        }else{
                            temm.Add(k);
                        }
                    }
                    if(key == null){
                        Console.WriteLine("This Feature cannot be styled");
                    }
                    else if(s2.Equals("1")){
                        key.setStyle("fill.black@stroke.black");
                    }else if(s2.Equals("2")){
                        key.setStyle("fill.blue@stroke.blue");
                    }else if(s2.Equals("3")){
                        key.setStyle("fill.green@stroke.green");
                    }else{
                        key.setStyle("fill.red@stroke.red");
                    }
                    temm.Add(key);
                    Memento this1=new Memento();
                    this1.SetState(temm);
                    face.SetState(this1);
                }
            }else if(s1.Contains("|") ){
                string [] breaker = s1.Split("|");
                s1=breaker[0];
                ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                Shape key=null;
                foreach(Shape i in nn){
                    if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                        key=i;
                    }else{
                        temm.Add(i);
                    }
                }
                if(key ==null){
                    Console.WriteLine("This feature cannot be styled");
                }
                else if(s2.Equals("1")){
                    key.setStyle("fill.black@stroke.black");
                }else if(s2.Equals("2")){
                    key.setStyle("fill.blue@stroke.blue");
                }else if(s2.Equals("3")){
                    key.setStyle("fill.green@stroke.green");
                }else{
                    key.setStyle("fill.red@stroke.red");
                }
                temm.Add(key);
                Memento this1=new Memento();
                this1.SetState(temm);
                face.SetState(this1);
            }else if(s2.Contains("|")){
                string [] breaker=s2.Split("|");
                s2=breaker[0];
                ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                Shape key=null;
                foreach(Shape i in nn){
                    if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                        key=i;
                    }else{
                        temm.Add(i);
                    }
                }
                if(key ==null){
                    Console.WriteLine("This feature cannot be styled");
                }
                else if(s2.Equals("1")){
                    key.setStyle("fill.black@stroke.black");
                }else if(s2.Equals("2")){
                    key.setStyle("fill.blue@stroke.blue");
                }else if(s2.Equals("3")){
                    key.setStyle("fill.green@stroke.green");
                }else{
                    key.setStyle("fill.red@stroke.red");
                }
                temm.Add(key);
                Memento this1=new Memento();
                this1.SetState(temm);
                face.SetState(this1);
            }else{
                ArrayList temm= new ArrayList();ArrayList nn= face.GetState();
                Shape door=null;
                foreach(Shape i in nn){
                    if(i.GetType().ToString().ToLower().Contains(s1.Replace("-",""))){
                        door=i;
                    }else{
                        temm.Add(i);
                    }
                }
                if(door ==null){
                    Console.WriteLine("This feature cannot be styled");
                }
                if(s2.Equals("1")){
                    door.setStyle("fill.black@stroke.black");
                }else if(s2.Equals("2")){
                    door.setStyle("fill.blue@stroke.blue");
                }else if(s2.Equals("3")){
                    door.setStyle("fill.green@stroke.green");
                }else{
                    door.setStyle("fill.red@stroke.red");
                }
                temm.Add(door);
                Memento this1=new Memento();
                this1.SetState(temm);
                face.SetState(this1);
            }
        }
        public static void RemoveFacialFeatures(string toRemove){
            Console.WriteLine("Removed feature(s) from the face");
            if(toRemove.Contains("|")){
                string [] temppp= toRemove.Split("|");
                int pos=0;int count=0;
                bool foun =false;
                ArrayList tem= new ArrayList();ArrayList real = face.GetState();
                foreach(var j in temppp){
                    pos=0;foun=false;tem=new ArrayList();count=0;
                    foreach(var i in real){
                        if(i.ToString().ToLower().Contains(j.Replace("-",""))){
                            foun=true;pos=count;break;
                        }
                        count++; 
                }
                if(!foun) Console.WriteLine("The " + j+" cannot be removed");
                else{
                    Shape door =null;
                    int counter =0;
                    foreach(Shape i in real){
                        if(counter == pos){
                            door=i;
                        }
                    }
                    user.Action(door,face,"remove");
                    real.RemoveAt(pos);
                    foreach(var i in real)tem.Add(i);
                    Memento noreason =new Memento();
                    noreason.SetState(tem);
                    face.SetState(noreason);
                }
            }
            }else{
                int pos=0;int count=0;
                bool foun =false;
                ArrayList tem= new ArrayList();ArrayList real = face.GetState();
                foreach(var i in real){
                    if(i.ToString().ToLower().Contains(toRemove.Replace("-",""))){
                        foun=true;pos=count;break;
                    }
                    count++;
                }
                if(!foun) Console.WriteLine("This feature cannot be removed");
                else{
                    Shape door =null;
                    int counter =0;
                    foreach(Shape i in real){
                        if(counter == pos){
                            door=i;
                        }
                    }
                    user.Action(door,face,"remove");
                    real.RemoveAt(pos);
                    foreach(var i in real)tem.Add(i);
                    Memento noreason =new Memento();
                    noreason.SetState(tem);
                    face.SetState(noreason);
                }
            }
        }
        public static void ResetFacialFeatures(){
            ArrayList somany= face.GetState();
            ArrayList newar1= new ArrayList();
            foreach( var i in somany){
                if(i.ToString().Equals("Circle")){
                    newar1.Add(i);
                }
                else if(i.ToString().Equals("LeftEyeDecorator")){
                    newar1.Add(new LeftEyeDecorator(new Circle()));
                }else if(i.ToString().Equals("RightEyeDecorator")){
                    newar1.Add(new RightEyeDecorator(new Circle()));
                }else if(i.ToString().Equals("MouthDecorator")){
                    newar1.Add(new MouthDecorator(new Path()));
                }else if(i.ToString().Equals("LeftBrowDecorator")){
                    newar1.Add(new LeftBrowDecorator(new Path()));
                }else if(i.ToString().Equals("RightBrowDecorator")){
                    newar1.Add(new RightBrowDecorator(new Path()));
                }
            }
            Memento noreasonagain= new Memento();
            noreasonagain.SetState(newar1);
            face.SetState(noreasonagain);
        }
        public static void commandPrintout(){
            Console.WriteLine(("Commands:"));
            Console.WriteLine("\tHelp\t\t\t\t\tHelp - displays this message.");
            Console.WriteLine("\tAdd {feature|...}\t\t\tAdds feature(s) to the face.");
            Console.WriteLine("\tMove {feature|...} {direction|...}\tMoves the feature(s) in the direction(s) given.");
            Console.WriteLine("\tRemove {feature|...}\t\t\tRemove feature(s).");
            Console.WriteLine("\tUndo\t\t\t\t\tUndo last operation.");
            Console.WriteLine("\tRedo\t\t\t\t\tRedo last operation.");
            Console.WriteLine("\tReset\t\t\t\t\tReset the feature(s) position(s) to default.");
            Console.WriteLine("\tStyle {feature|...} {number|...}\tStyles feature(s) according to chosen style(s).");
            Console.WriteLine("\tDraw\t\t\t\t\tDisplays the face.");
            Console.WriteLine("\tSave {filename}\t\t\t\tSave your canvas to a specified svg file.");
            Console.WriteLine("\tQuit\t\t\t\t\tQuit application.");
        }
        public static void addShape(string value1,string value2,string value3,string value4,string shape,string id,string style,string filePath)
        {
            string svg="</svg>";
            string lines=File.ReadAllText(filePath);
            readWrite(filePath,svg);
            if(shape.Equals(" Circle")){
                using (var writer = File.AppendText(filePath))  
                {  
                    writer.Write("\t" +"<circle"+ " id="+@""""+id+@""""+" cx="+@""""+value1+@""""+" cy="+@""""+value2+@""""+" r="+@""""+value3+@""""+" style="+@""""+style+@""""+"/>" + Environment.NewLine);
                    writer.Write(svg);
                } 
            }else if(shape.Equals(" Path")){
                using (var writer = File.AppendText(filePath))  
                {  
                    writer.Write("\t" +"<path "+ " id="+@""""+id+@""""+" d="+@""""+value1+@""""+" style="+@""""+style+@""""+ "/>" +Environment.NewLine);
                    writer.Write(svg);
                } 
            }
        }
        public static string drawSVG(string [] tempAr)
        {
            string id="",cx="",cy="",r="",style="",d="",shape="",result="";
            for(int i=2;i<tempAr.Length;i++){
                if(tempAr[i].Contains(":")){
                    string [] temp= tempAr[i].Split(" (");string [] temp2= tempAr[i].Split(":");
                    shape=temp[0].Replace(">","").Replace(" ","");string shapes=RemoveSpaces(shape);
                    if(shapes.Equals("Circle")){
                        string [] ar1=temp2[1].Split(",");id=ar1[0].Replace(" ","");
                        string [] ar2=temp2[2].Split(",");cx=ar2[0].Replace(" ","");
                        string [] ar3=temp2[3].Split(",");cy=ar3[0].Replace(" ","");
                        string [] ar4=temp2[4].Split(",");r=ar4[0].Replace(" ","");
                        style=temp2[5].Replace(".",":").Replace(")","").Replace(" ","").Replace("@",";");
                    }else if(shapes.Equals("Path")){
                        string [] ar1=temp2[1].Split(",");id=ar1[0].Replace(" ","");
                        d=temp2[2].Replace("  , style","");
                        style=temp2[3].Replace(".",":").Replace(")","").Replace(" ","").Replace("@",";");
                    }
                    if(shape.Equals("Circle")){
                            result +="\t"+">  " +"<circle"+ " id="+@""""+id+@""""+" cx="+@""""+cx+@""""+" cy="+@""""+cy+@""""+" r="+@""""+r+@""""+" style="+@""""+style+@""""+"/>" + Environment.NewLine;
                    }else if(shape.Equals("Path")){
                            result +="\t"+">  " +"<path "+ " id="+@""""+id+@""""+" d="+@""""+d+@""""+" style="+@""""+style+@""""+ "/>" +Environment.NewLine;
                    }
                }     
            }
            return result;
        }
        public static void readWrite(string filePath,string context){
            string text;
            string value="";
            StreamReader sr = File.OpenText(filePath);
            while ((text = sr.ReadLine()) != null)
            {
                //Add it to the value string if it doesn't have the closing tag
                if (!text.Contains(context))
                {
                    value += text + Environment.NewLine;
                }
            }
            //Close the reader
            sr.Close();
            //Write the string version without the closing tag
            File.WriteAllText(filePath, value);
        }
    }

