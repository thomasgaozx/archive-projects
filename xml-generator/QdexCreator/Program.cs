using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.IO.Compression;

using qdex.QdexCreator.Nodes;
using System.IO;
using System.Reflection;

namespace qdex.QdexCreator
{
    public class Program
    {
        public static XmlTextWriter writer;
        public static Random random = new Random();

        #region Random Utility

        public static T PopRandom<T>(List<T> list)
        {
            return list[random.Next(list.Count)];
        }

        public static T Switch<T>(T t1, T t2)
        {
            return random.NextDouble() < 0.5 ? t1 : t2;
        }

        #endregion

        #region Main

        static void Main()
        {
            try
            {

                // Set Up
                var dirInfo = Directory.CreateDirectory("test_module");
                dirInfo.CreateSubdirectory("resources");
                dirInfo.CreateSubdirectory("Equations");

                writer = new XmlTextWriter(@"test_module\document.xml", Encoding.UTF8);



                // Raw resources
                List<string> imgLocs = new List<string> { "Aero.jpg", "Gyro.jpg", "Hexapod.jpg", "QBall.jpg", "QBot.jpg", "Qdex.png", "Qdex2.png", "QDrone.png", "QUBE.jpg" };
                List<string> imgSize = new List<string> { "xlarge", "large", "medium", "small", "tiny" };

                List<string> paragraphs = new List<string>
                {
                    "THE mobile environment for STEM education",
                    "Create your own educational mobile apps",
                    "qdex makes it easy to build custom mobile applications for iOS and Android. With our simple-to-use creation tool, you can go from a blank slate to a fully-functioning native mobile application in minutes. No development experience required.",
                    "qdex isn't just mobile friendly, it's mobile powered. We've designed qdex exclusively for mobile devices to let you harness the full extent of their computational power, allowing you to deliver content in a more engaging way than ever before.",
                    "Go beyond flash animations and pseudo simulations. With advanced mathematical solvers, communication streams, and hardware-accelerated graphics, qdex is a step above traditional eLearning platforms.",
                    "Remotely control any hardware with the tap of a button. Send commands, receive data, display the response. Our real-time communications tools make feedback control applications accessible. ",
                    "qdex makes it easy to create the apps that you need. With our authoring tool and dynamic approach to content creation, even the most inexperienced creators can develop feature-rich modules in a matter of minutes.",
                    "qdex utilizes a robust scripting language called Lua to introduce customizability into applications. Lua supports most of the popular programming paradigms, allowing you to create large-scale cross-platform applications without hours of development time.",
                    "Dynamically alter lessons to suit your student's pace or skill level, jump between concepts with ease, and show/hide content based on user feedback. qdex gives you the flexibility to create content that moves the way you want it to.",
                    "Once you have segmented your course into the core concepts that require interactive app components, your next step is to storyboard the interactions to layout the experience for your students. Your storyboards can scale in complexity from simple hand drawings, through to detailed renderings and user experience stories. The general idea is that you want to decide what you would like your student to experience, and how their interactions will lead to a better understanding of the concept." +
                    "\nIt is important that you do not limit the complexity of the experience you would like your students to have at this stage.You should let your creativity run wild and imagine the best experience for your students, and then leave the logistics to the development stage."
                };

                List<string> sectionTitles = new List<string>
                {
                    "Development Guide",
                    "Creating effective qdex modules",
                    "Design Guidelines",
                    "Aesthetics",
                    "Organization",
                    "Communicating Concepts",
                    "Developing Modules",
                    "Bulleted Lists",
                    "Attributes, Methods, and Events",
                    "Defining Your Model",
                    "Math, Logic, Compare",
                    "Recommended Authoring Approach",
                    "Distributing Modules",
                    "Hardware Communications",
                    null,
                    null
                };


                HashSet<string> names = new HashSet<string>();
                // Primary Resources
                ImageNode[] images = new ImageNode[random.Next(2, 15)];
                for (int i = 0; i < images.Length; i++)
                {
                    string name = PopRandom(imgLocs);
                    if (!names.Contains(name))
                    {
                        names.Add(name);
                        File.Copy(@"Resources\" + name, @"test_module\resources\"+name, true);
                    }
                    images[i] = new ImageNode(writer)
                    {
                        Src = @"resources\"+name,
                        Style = PopRandom(imgSize)
                    };
                }

                PNode[] ps = new PNode[random.Next(10, 25)];
                for (int i = 0; i < ps.Length; i++)
                {
                    ps[i] = new PNode(writer).AppendContent(PopRandom(paragraphs));
                }

                string[] secs = new string[random.Next(5, 8)];
                for (int i = 0; i < secs.Length; i++)
                {
                    secs[i] = PopRandom(sectionTitles);
                }

                StackNode[] stacks = new StackNode[secs.Length];
                int curImgIndex = 0;
                int curPIndex = 0;
                for (int i = 0; i < stacks.Length-1; i++)
                {
                    stacks[i] = new StackNode(writer);

                    // distribute images
                    int maxImgIndexIncre = images.Length - curImgIndex - stacks.Length+i;
                    if (maxImgIndexIncre>0)
                    {
                        int randImgIncre = random.Next(1, maxImgIndexIncre>4?4:maxImgIndexIncre);
                        for (int j = curImgIndex; j < curImgIndex+randImgIncre; j++)
                        {
                            stacks[i].AppendNode(images[j]);
                        }
                        curImgIndex += randImgIncre;
                    }

                    // distribute paragraphs
                    int maxPIndexIncre = ps.Length - curPIndex - stacks.Length + i;
                    int randPIncre = random.Next(1, maxPIndexIncre>6?6:maxPIndexIncre);
                    for (int j = curPIndex; j < curPIndex+randPIncre; j++)
                    {
                        stacks[i].AppendNode(ps[j]);
                    }
                    curPIndex += randPIncre;

                    stacks[i].RandSwap(random);
                }

                stacks[stacks.Length - 1] = new StackNode(writer);
                if (curImgIndex <= images.Length - 1)
                {
                    for (int k = curImgIndex; k < images.Length; k++)
                    {
                        stacks[stacks.Length - 1].AppendNode(images[k]);
                    }
                }
                for(int k=curPIndex; k<ps.Length;k++)
                {
                    stacks[stacks.Length - 1].AppendNode(ps[k]);
                }
                stacks[stacks.Length - 1].RandSwap(random);

                // Creating the Document
                DocumentRoot doc = new DocumentRoot(writer);
                foreach(string title in secs)
                {
                    if (title==null)
                    {
                        doc.AddSection();
                    }
                    else
                    {
                        doc.AddSection(title);
                    }
                }

                for (int i=0; i<stacks.Length; i++)
                {
                    doc.Section(i).AppendNode(stacks[i]);
                }


                // Default
                doc.WriteNode();
                writer.Close();
                File.Copy(@"test_module\document.xml", @"test_module\source.xml",true);
                if (File.Exists("test_module.zip"))
                {
                    File.Delete("test_module.zip");
                }
                ZipFile.CreateFromDirectory("test_module", "test_module.zip", CompressionLevel.Fastest, false);
                MessageBox.Show("XML File created!");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Message: {e.Message}\nSource: {e.Source}\n\nStackTrace\n{e.StackTrace}");
            }
            finally
            {
                // Tear Down
                if (writer!=null)
                {
                    writer.Close();
                }
            }

        }

        #endregion

    }
}
