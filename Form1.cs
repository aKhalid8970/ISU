// Abdullah Khalid
// Grade 11 Comp Sci, Afternoon
// June 17, 2024
// Final Project
/*
 * PURPOSE
 * The purpose of coding tetris is to really push my limits as far as my logic goes behind coding
 * I wanted to try a game which will challenge me and make me use as much ideas and concepts we learned this year
 * The game involved some new things which I know from previous coding knowledge and from the Internet such as 2d arrarys and data structs
 * There are if statements, for loops, try/except statements, various variable types, various form classes, event handling, etc.
 * While the game is buggy even as I am submitting this, I am still proud of the game right now, and I will definitely be looking finish this in the summer
 * Refer to the external comments to know what errors are still present in case you want to play without any faults.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BRAINSTORM
{
    public partial class Form1 : Form
    {
        // set int counter equal to 0
        int counter = 0;

        // set int left checker equal to 0
        int leftChecker = 0;

        // set int right checker equal to 0
        int rightChecker = 0;

        // declare a rect var called border
        Rectangle border;

        bool keyUpValid = true;

        // set int x and y equal to 0
        int x = 0;
        int y = 0;

        // create a picbox called title
        PictureBox title;

        // create an image called tetris
        Image tetris;

        // Lets compiler know what shape is currently being used
        int shape;

        // lets compiler know what rotation state shape being used is in
        int rotation;

        // set int z equal to 0, used for a buffervalue for updating the score
        int z;

        // set int rows equal to 20
        int rows = 20;

        // set int columns equal to 10
        int columns = 10;

        // create a slow timer
        Timer slow;

        // create a fast timer
        Timer fast;

        // create label called score and controls
        Label score, controls;

        // set rand equal to new random type
        Random rand = new Random();

        // declare rect called background
        Rectangle background;

        /*
         * Create a data structure called gridbox
         * Each variable which is a data structure will have the indented properties to it
         * Reference: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/struct
         */
        public struct gridBox
        {
            // declare rect as rectangle var
            public Rectangle rect;
            
            //declare bool var called inuse
            public bool inUse;

            // declare status as int
            public int status;

            // declare x and y as int
            public int x;
            public int y;

            // declare rotate block as int
            public int rotateBlock;

        }

        // set int pts equal to 0
        int pts = 0;

        /*
         * declare maingrid as gridBox data type, and declare it as a two dimensional array
         * essentially this is going to model the tetris grid frame being 20 x 10 boxes
         * each box will have a property of gridBox
         * Reference: https://www.w3schools.com/cs/cs_arrays_multi.php
         */
        gridBox[,] mainGrid = new gridBox[20, 10];

        // create a bufferGrid to track movements and rotations of shapes
        gridBox[,] bufferGrid = new gridBox[20, 10];
        
        // intitalize form
        public Form1()
        {
            InitializeComponent();
        }

        // form load
        private void Form1_Load(object sender, EventArgs e)
        {
            // set height and width equal to 600 for form
            this.Height = 600;
            this.Width = 600;

            // set background color to black
            this.BackColor = Color.Black;

            // set tetris equal to a tetris image form the web
            tetris = Image.FromFile(Application.StartupPath + @"\tetris.png", true);

            // set title equal to a new pic box
            title = new PictureBox();

            // set title coordinates to 1, 1
            title.Top = 1;
            title.Left = 1;

            // set title as the size of the tetris image
            title.Size = tetris.Size;

            // set the title background image to tetris
            title.BackgroundImage = tetris;

            // add the title to form
            this.Controls.Add(title);

            // set controls equal to new label
            controls = new Label();

            // set controls coordinates to 300, 370
            controls.Top = 370;
            controls.Left = 300;

            // set controls dimensions to 100x200
            controls.Height = 100;
            controls.Width = 200;

            // set text to controls as the instructions on what controls to use for the game
            controls.Text = "Up arrow to rotate, left and right arrows to move block left and right respectively";

            // set controls font to arial, 14 size, regular font style
            controls.Font = new Font("Arial", 14, FontStyle.Regular);

            // set the controls back colour to white
            controls.BackColor = Color.White;

            // add controls to form
            this.Controls.Add(controls);

            // set score equal to a new variable
            score = new Label();

            // set score coordinates to 325, 175
            score.Top = 175;
            score.Left = 325;

            // set score width and height to 100
            score.Height = 100;
            score.Width = 100;

            // set scores text to pts
            score.Text = "Score: " + pts.ToString();

            // set font to arial, 14, and regular of score text
            score.Font = new Font("Arial", 14, FontStyle.Regular);

            // set background colour of score to white
            score.BackColor = Color.White;

            // add score to form
            this.Controls.Add(score);
            
            // set double buffered to true for painting rects
            this.DoubleBuffered = true;

            // add painting method to paint of form
            this.Paint += Form1_Paint;

            // set background equal to new rect at 300, 150, 150x200
            background = new Rectangle(300, 150, 150, 200);
            
            // create border as new rect at 25, 150, 300x4000
            border = new Rectangle(25, 150, 300, 400);

            // iterate through rows
            for (int i = 0; i < rows; i++)
            {
                // set x equal to 0
                x = 0;

                // iterate through columns
                for (int j = 0; j < columns; j++)
                {
                    /*
                     * set maingrid and buffergrid at i, j to a new rect at 26+x, 151+y, 20 x 20
                     * set their inuse to false, status and rotateblock to 0
                     * set their x values to 26 + x, and set their y values to 151 + y
                     */

                    // create new rect at i, j for main grid, with coordinates of 26 +x, 151 + y, 20x20 dimensions
                    mainGrid[i, j].rect = new Rectangle(26 + x, 151 + y, 20, 20);

                    // set maingrid at i, j to false in use
                    mainGrid[i, j].inUse = false;

                    // set status of maingrid at i, j to 0
                    mainGrid[i, j].status = 0;

                    // set rotateblock of maingrid at i, j to 0
                    mainGrid[i, j].rotateBlock = 0;

                    // set x value of maingrid i, j to 26 + x
                    mainGrid[i, j].x = 26 + x;

                    // set y value of maingrid i, j to 151 + y
                    mainGrid[i, j].y = 151 + y;

                    // create new rect at i, j for buffer grid, with coordinates of 26 +x, 151 + y, 20x20 dimensions
                    bufferGrid[i, j].rect = new Rectangle(26 + x, 151 + y, 20, 20);

                    // set buffergrid at i, j to false in use
                    bufferGrid[i, j].inUse = false;

                    // set status of buffergrid at i, j to 0
                    bufferGrid[i, j].status = 0;

                    // set rotateblock of buffergrid at i, j to 0
                    bufferGrid[i, j].rotateBlock = 0;

                    // set x value of buffergrid i, j to 26 + x
                    bufferGrid[i, j].x = 26 + x;

                    // set y value of buffergrid i, j to 151 + y
                    bufferGrid[i, j].y = 151 + y;

                    // add 20 to x
                    x += 20;
                }

                // add 20 to y
                y += 20;
            }
            

            // set key down method
            this.KeyDown += Form1_KeyDown;

            // set key up method
            this.KeyUp += Form1_KeyUp;
            
            // set slow to new timer
            slow = new Timer();
            
            // set slow interval to 500
            slow.Interval = 500;

            // set up slow timer tick method
            slow.Tick += Slow_Tick;

            // start slow timer
            slow.Start();

            // set fast to new timer
            fast = new Timer();

            // set fast interval to 60 fps
            fast.Interval = 1000 / 60;

            // set up fast timer tick method
            fast.Tick += Fast_Tick;

            // start fast timer
            fast.Start();
            
        }

        // fast timer tick method
        private void Fast_Tick(object sender, EventArgs e)
        {

            // set z equal to return value of clear method
            z = clear();

            // if z is greater than 0
            if (z > 0)
            {

                // take one away from z
                z = z - 1;

                // decrease the slow interval by 25
                slow.Interval = slow.Interval - 25;

            }

            // increment pts by z
            pts += z;
            
            // refresh the text of score
            score.Text = "Score: " + pts.ToString();

            // refresh 
            this.Invalidate();
        }

        // key up method
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            // if left arrow is released
            if (e.KeyCode == Keys.Left)
            {

                // if left checker is 0
                if (leftChecker == 0)
                {

                    // call mainToBuffer method
                    mainToBuffer();

                    // start slow timer
                    slow.Start();

                    // set left checker to 0
                    leftChecker = 0;

                }
            }

            // if right arrow is released
            if (e.KeyCode == Keys.Right)
            {

                // if right checker is 0
                if (rightChecker == 0)
                {

                    //call mainToBuffer method
                    mainToBuffer();

                    // start slow timer
                    slow.Start();

                    // set right checker to 0
                    rightChecker = 0;
                }
                
                
            }

            // if up arrow is released
            if (e.KeyCode == Keys.Up)
            {
                if (keyUpValid == true && shape != 0)
                {
                    //if rotation is in its last state
                    if (rotation == 3)
                    {
                        // reset rotation state
                        rotation = 0;
                    }

                    // else
                    else
                    {
                        // increment rotation by 1
                        rotation++;
                    }

                    //call mainToBuffer method
                    mainToBuffer();

                    // start slow timer
                    slow.Start();

                }
                
            }

        }

        // key down method
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // if up pressed
            if (e.KeyCode == Keys.Up)
            {

                // if shape is not the 2x2 block
                if (shape != 0)
                {
                    // iterate through rows
                    for (int r = 0; r < rows; r++)
                    {
                        // iterate through columns
                        for (int c = 0; c < columns; c++)
                        {

                            // if maingrid at r, c is being used by user
                            if (mainGrid[r, c].inUse == true)
                            {

                                // try the indented code
                                try
                                {
                                    // stop slow timer
                                    slow.Stop();

                                    // if t shape is being used
                                    if (shape == 1)
                                    {

                                        // if rotation in first stage
                                        if (rotation == 0)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {
                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 1;
                                            }
                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 3;
                                            }
                                            // else if rotate block is 4
                                            else if (mainGrid[r, c].rotateBlock == 4)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 4;
                                            }

                                            // else if rotate block at r, c is 2
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 2;
                                            }
                                        }

                                        // if rotation in second stage
                                        else if (rotation == 1)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 3;
                                            }
                                            // else if rotate block is 4
                                            else if (mainGrid[r, c].rotateBlock == 4)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 4;
                                            }

                                            // else if rotate block at r, c is 2
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 2;
                                            }
                                        }
                                        else if (rotation == 2)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 3;
                                            }
                                            // else if rotate block is 4
                                            else if (mainGrid[r, c].rotateBlock == 4)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 4;
                                            }

                                            // else if rotate block at r, c is 2
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 2;
                                            }
                                        }

                                        // if rotation in 4th stage
                                        else
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else if (mainGrid[r, c].rotateBlock == 4)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 4;
                                            }

                                            // else if rotate block at r, c is 2
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 2;
                                            }
                                        }
                                    }

                                    // if I shape is being used
                                    else if (shape == 2)
                                    {

                                        // if in first rotation 
                                        if (rotation == 0)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 1, c + 2].inUse = true;
                                                bufferGrid[r - 1, c + 2].status = 1;
                                                bufferGrid[r - 1, c + 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c + 1].inUse = true;
                                                bufferGrid[r, c + 1].status = 1;
                                                bufferGrid[r, c + 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r + 1, c].inUse = true;
                                                bufferGrid[r + 1, c].status = 1;
                                                bufferGrid[r + 1, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 2, c - 1].inUse = true;
                                                bufferGrid[r + 2, c - 1].status = 1;
                                                bufferGrid[r + 2, c - 1].rotateBlock = 4;
                                            }
                                        }

                                        // if in second rotation
                                        else if (rotation == 1)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 2, c + 1].inUse = true;
                                                bufferGrid[r + 2, c + 1].status = 1;
                                                bufferGrid[r + 2, c + 1].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r + 1, c].inUse = true;
                                                bufferGrid[r + 1, c].status = 1;
                                                bufferGrid[r + 1, c].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c - 1].inUse = true;
                                                bufferGrid[r, c - 1].status = 1;
                                                bufferGrid[r, c - 1].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c - 2].inUse = true;
                                                bufferGrid[r - 1, c - 2].status = 1;
                                                bufferGrid[r - 1, c - 2].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in third stage
                                        else if (rotation == 2)
                                        {

                                            // if rotate block at r, c is 1
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 1, c - 2].inUse = true;
                                                bufferGrid[r + 1, c - 2].status = 1;
                                                bufferGrid[r + 1, c - 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r, c - 1].inUse = true;
                                                bufferGrid[r, c - 1].status = 1;
                                                bufferGrid[r, c - 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r - 1, c].inUse = true;
                                                bufferGrid[r - 1, c].status = 1;
                                                bufferGrid[r - 1, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 2, c + 1].inUse = true;
                                                bufferGrid[r - 2, c + 1].status = 1;
                                                bufferGrid[r - 2, c + 1].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in 4th stage
                                        else
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 2, c - 1].inUse = true;
                                                bufferGrid[r - 2, c - 1].status = 1;
                                                bufferGrid[r - 2, c - 1].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r - 1, c].inUse = true;
                                                bufferGrid[r - 1, c].status = 1;
                                                bufferGrid[r - 1, c].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c + 1].inUse = true;
                                                bufferGrid[r, c + 1].status = 1;
                                                bufferGrid[r, c + 1].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c + 2].inUse = true;
                                                bufferGrid[r + 1, c + 2].status = 1;
                                                bufferGrid[r + 1, c + 2].rotateBlock = 4;
                                            }
                                        }
                                    }

                                    // else if L shape is being used
                                    else if (shape == 3)
                                    {

                                        // if rotation in first stage
                                        if (rotation == 0)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 2, c].inUse = true;
                                                bufferGrid[r - 2, c].status = 1;
                                                bufferGrid[r - 2, c].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 4;
                                            }
                                        }

                                        // if in second rotation
                                        else if (rotation == 1)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r, c + 2].inUse = true;
                                                bufferGrid[r, c + 2].status = 1;
                                                bufferGrid[r, c + 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in third stage
                                        else if (rotation == 2)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 2, c].inUse = true;
                                                bufferGrid[r + 2, c].status = 1;
                                                bufferGrid[r + 2, c].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in 4th stage
                                        else
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r, c - 2].inUse = true;
                                                bufferGrid[r, c - 2].status = 1;
                                                bufferGrid[r, c - 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 4;
                                            }
                                        }
                                    }

                                    // else if J shape is being used
                                    else if (shape == 4)
                                    {

                                        // if rotation in first stage
                                        if (rotation == 0)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r, c - 2].inUse = true;
                                                bufferGrid[r, c - 2].status = 1;
                                                bufferGrid[r, c - 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 4;
                                            }
                                        }

                                        // if in second rotation
                                        else if (rotation == 1)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r - 2, c].inUse = true;
                                                bufferGrid[r - 2, c].status = 1;
                                                bufferGrid[r - 2, c].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in third stage
                                        else if (rotation == 2)
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r, c + 2].inUse = true;
                                                bufferGrid[r, c + 2].status = 1;
                                                bufferGrid[r, c + 2].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r - 1, c + 1].inUse = true;
                                                bufferGrid[r - 1, c + 1].status = 1;
                                                bufferGrid[r - 1, c + 1].rotateBlock = 2;
                                            }

                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r + 1, c - 1].inUse = true;
                                                bufferGrid[r + 1, c - 1].status = 1;
                                                bufferGrid[r + 1, c - 1].rotateBlock = 4;
                                            }
                                        }

                                        // if rotation in 4th stage
                                        else
                                        {

                                            // Set buffergrid up at r, c to rotate counter clockwise, with the exact new coordinates dependant on its rotateblock
                                            if (mainGrid[r, c].rotateBlock == 1)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 1
                                                 */
                                                bufferGrid[r + 2, c].inUse = true;
                                                bufferGrid[r + 2, c].status = 1;
                                                bufferGrid[r + 2, c].rotateBlock = 1;
                                            }

                                            // else if rotate block at r, c is 2
                                            else if (mainGrid[r, c].rotateBlock == 2)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 2
                                                 */
                                                bufferGrid[r + 1, c + 1].inUse = true;
                                                bufferGrid[r + 1, c + 1].status = 1;
                                                bufferGrid[r + 1, c + 1].rotateBlock = 2;
                                            }


                                            // else if rotate block at r, c is 3
                                            else if (mainGrid[r, c].rotateBlock == 3)
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 3
                                                 */
                                                bufferGrid[r, c].inUse = true;
                                                bufferGrid[r, c].status = 1;
                                                bufferGrid[r, c].rotateBlock = 3;
                                            }

                                            // else if rotate block is 4
                                            else
                                            {

                                                /*
                                                 * Put new coordinates for buffer grid depending on rotation
                                                 * set its inUse to true
                                                 * set its status to 1 and rotateBlock to 4
                                                 */
                                                bufferGrid[r - 1, c - 1].inUse = true;
                                                bufferGrid[r - 1, c - 1].status = 1;
                                                bufferGrid[r - 1, c - 1].rotateBlock = 4;
                                            }
                                        }
                                    }

                                    // set keyUpValid to true
                                    keyUpValid = true;
                                }

                                // unless if index out of range exception occurs
                                catch (IndexOutOfRangeException)
                                {

                                    // set keyUpValid to false
                                    keyUpValid = false;

                                    // call reset buffer method
                                    resetBuffer();

                                    // exit method
                                    return;
                                }
                            }
                        }
                    }
                
                }
            }

            // if left key is pressed
            if (e.KeyCode == Keys.Left)
            {
                // iterate through rows
                for (int i = 0; i < rows; i++)
                {

                    // if any block on the left hand column is in use
                    if (mainGrid[i, 0].inUse == true)
                    {

                        // increment left checker
                        leftChecker++;
                    }
                }

                if (leftChecker == 0)
                {
                    // iterate through rows
                    for (int r = 0; r < rows; r++)
                    {
                        // iterate through columns
                        for (int c = 0; c < columns; c++)
                        {

                            // if block at r, c is being used and c does not equal 0
                            if (mainGrid[r, c].inUse == true && c != 0)
                            {

                                // stop slow timer
                                slow.Stop();
                                
                                // set buffergrid to the left of r, c's status to 1
                                bufferGrid[r, c - 1].status = 1;

                                // set buffer grid for the left of r, c's inuse to true
                                bufferGrid[r, c - 1].inUse = true;

                                // set buffer grid to the left of r, c's rotate blcok to main grid's rotate block at r, c
                                bufferGrid[r, c - 1].rotateBlock = mainGrid[r, c].rotateBlock;
                            }
                        }
                    }
                }
                
            }

            // if right key is pressed
            if (e.KeyCode == Keys.Right)
            {
                // iterate through rows
                for (int i = 0; i < rows; i++)
                {
                    
                    // if any block on right hand side is being used
                    if (mainGrid[i, 9].inUse == true)
                    {

                        // increment right checker
                        rightChecker++;
                    }
                }

                // if right checker is 0
                if (rightChecker == 0)
                {
                    // iterate through rows
                    for (int r = 0; r < rows; r++)
                    {
                        // iterate through columns
                        for (int c = 0; c < columns; c++)
                        {
                            if (mainGrid[r, c].inUse == true)
                            {

                                // stop slow timer
                                slow.Stop();

                                // set buffergrid to the right of r, c's status to 1
                                bufferGrid[r, c + 1].status = 1;

                                // set buffer grid to the right of r, c's inuse to true
                                bufferGrid[r, c + 1].inUse = true;

                                // set buffer grid to the right of r, c's rotate block to maingrids rotate block at r, c
                                bufferGrid[r, c + 1].rotateBlock = mainGrid[r, c].rotateBlock;
                            }
                        }
                    }
                }
            }
        }

        // slow timer tick method
        private void Slow_Tick(object sender, EventArgs e)
        {

            // refresh
            this.Invalidate();

            // call inProcess method
            inProcess();

            // refresh
            this.Invalidate();
        }

        
        // in process method, no return types or parameters
        public void inProcess()
        {

            // if counter is 0
            if (counter == 0)
            {

                // call create method to generate new block
                create();

                // exit method
                return;
            }

            // else
            else
            {
                // iterate through rows
                for (int r = 0; r < rows; r++)
                {
                    // iterate through columns
                    for (int c = 0; c < columns; c++)
                    {
                        
                        // if maingrid at r, c is being used right now
                        if (mainGrid[r, c].status == 1 && mainGrid[r, c].inUse == true && mainGrid[r, c].rotateBlock > 0)
                        {

                            // if maingrid block at r, c is about to hit the bottom of the grid or about to place on top of a formerly used block
                            if (r == 19 || (mainGrid[r + 1, c].status == 1 && mainGrid[r + 1, c].inUse == false))
                            {

                                // call reset status method
                                resetStatus();

                                // call create method to generate block
                                create();
                                
                                // exit method
                                return;
                            }
                        }
                    }
                }
                // iterate through rows
                for (int r = 0; r < rows; r++)
                {
                    // iterate through columns
                    for (int c = 0; c < columns; c++)
                    {
                        // if block at r, c is being used and is not about to touch bottom of grid
                        if (mainGrid[r, c].status == 1 && mainGrid[r, c].inUse == true && r < 19)
                        {

                            // set buffer grid to main grid specs but a row below it
                            bufferGrid[r + 1, c].status = 1;
                            bufferGrid[r + 1, c].inUse = true;
                            bufferGrid[r + 1, c].rotateBlock = mainGrid[r, c].rotateBlock;
                        }
                    }
                }
                // iterate through rows
                for (int r = 0; r < rows; r++)
                {
                    // iterate through columns
                    for (int c = 0; c < columns; c++)
                    {

                        // if block at r, c was formerly used
                        if (mainGrid[r, c].status == 1 && mainGrid[r, c].inUse == false)
                        {

                            // do nothing
                            counter += 0;
                        }

                        // else
                        else
                        {

                            // set main grid at r, c to what buffer grid is at r, c
                            mainGrid[r, c].status = bufferGrid[r, c].status;
                            mainGrid[r, c].inUse = bufferGrid[r, c].inUse;
                            mainGrid[r, c].rotateBlock = bufferGrid[r, c].rotateBlock;
                        }

                    }
                }

                // repaint screen
                this.Paint += Form1_Paint1;

                // call reset buffer method
                resetBuffer();

                // iterate through rows
                for (int r = 0; r < rows; r++)
                {

                    // iterate through columns
                    for (int c = 0; c < columns; c++)
                    {


                        // create new rect for buffergrid and maingrid at r, c with existing x and y coordinates, 20x20 dimensions
                        mainGrid[r, c].rect = new Rectangle(mainGrid[r, c].x, mainGrid[r, c].y, 20, 20);
                        bufferGrid[r, c].rect = new Rectangle(bufferGrid[r, c].x, bufferGrid[r, c].y, 20, 20);
                    }
                }
            }



        }


        // reset status method, no return types or parameters
        public void resetStatus()
        {
            // iterate through rows
            for (int r = 0; r < rows; r++)
            {
                // iterate through columns
                for (int c = 0; c < columns; c++)
                {

                    // if block at r, c is being used
                    if (mainGrid[r, c].status == 1 && mainGrid[r, c].inUse == true)
                    {

                        // make its inuse to false
                        mainGrid[r, c].inUse = false;
                    }
                    
                }
            }
        }

        // create method, no return value, no parameter
        public void create()
        {
            

            // it first two rows empty
            if (isRowEmpty(0) == true && isRowEmpty(1) == true)
            {
                // set counter to 1
                counter = 1;

                // generate random blocl
                generate(rand.Next(0, 4));
            }

            //else
            else
            {

                // stop slow timer
                slow.Stop();

                // message box showing game over, ending game
                MessageBox.Show("Game Over");
            }
        }

        // declare isRowEmpty method returning a bool value, taking int r as a parameter
        public bool isRowEmpty(int r)
        {

            // iterate thru columns
            for (int c = 0; c < columns; c++)
            {

                // if maingrid at r, c is not filled with a block
                if (mainGrid[r, c].status != 0)
                {

                    // return false
                    return false;
                }
            }

            // return true
            return true;
        }

        public void generate(int rand)
        {
            // set rotation equal to 0
            rotation = 0;

            // if rand is 0
            if (rand == 0)
            {

                /*
                 * create 2x2 block at top of grid
                 * set each block's status to 1 and inuse to true
                 * Create 4 separate integers for rotateblock so it can be used for rotating
                 */
                mainGrid[0, 4].status = 1;
                mainGrid[0, 4].inUse = true;
                mainGrid[0, 4].rotateBlock = 1;
                mainGrid[0, 5].status = 1;
                mainGrid[0, 5].inUse = true;
                mainGrid[0, 5].rotateBlock = 2;
                mainGrid[1, 4].status = 1;
                mainGrid[1, 4].inUse = true;
                mainGrid[1, 4].rotateBlock = 3;
                mainGrid[1, 5].status = 1;
                mainGrid[1, 5].inUse = true;
                mainGrid[1, 5].rotateBlock = 4;

                // set shape to 0
                shape = 0;
            }

            // if rand is 1
            if (rand == 1)
            {
                /*
                 * create t block at top of grid
                 * set each block's status to 1 and inuse to true
                 * Create 4 separate integers for rotateblock so it can be used for rotating
                 */
                mainGrid[1, 4].status = 1;
                mainGrid[1, 4].inUse = true;
                mainGrid[1, 4].rotateBlock = 1;
                mainGrid[2, 4].status = 1;
                mainGrid[2, 4].inUse = true;
                mainGrid[2, 4].rotateBlock = 2;
                mainGrid[2, 3].status = 1;
                mainGrid[2, 3].inUse = true;
                mainGrid[2, 3].rotateBlock = 3;
                mainGrid[2, 5].status = 1;
                mainGrid[2, 5].inUse = true;
                mainGrid[2, 5].rotateBlock = 4;

                // set shape to 1 to indicate to compiler what shape is being used
                shape = 1;

            }

            // if rand is 2
            if (rand == 2)
            {
                /*
                 * create I block at top of grid
                 * set each block's status to 1 and inuse to true
                 * Create 4 separate integers for rotateblock so it can be used for rotating
                 */
                mainGrid[1, 3].status = 1;
                mainGrid[1, 3].inUse = true;
                mainGrid[1, 3].rotateBlock = 1;
                mainGrid[1, 4].status = 1;
                mainGrid[1, 4].inUse = true;
                mainGrid[1, 4].rotateBlock = 2;
                mainGrid[1, 5].status = 1;
                mainGrid[1, 5].inUse = true;
                mainGrid[1, 5].rotateBlock = 3;
                mainGrid[1, 6].status = 1;
                mainGrid[1, 6].inUse = true;
                mainGrid[1, 6].rotateBlock = 4;

                // set shape to 2 to indicate to compiler what shape is being used
                shape = 2;

            }

            // if rand is 3
            if (rand == 3)
            {
                /*
                 * create L block at top of grid
                 * set each block's status to 1 and inuse to true
                 * Create 4 separate integers for rotateblock so it can be used for rotating
                 */
                mainGrid[1, 3].status = 1;
                mainGrid[1, 3].inUse = true;
                mainGrid[1, 3].rotateBlock = 2;
                mainGrid[1, 4].status = 1;
                mainGrid[1, 4].inUse = true;
                mainGrid[1, 4].rotateBlock = 3;
                mainGrid[1, 5].status = 1;
                mainGrid[1, 5].inUse = true;
                mainGrid[1, 5].rotateBlock = 4;
                mainGrid[2, 3].status = 1;
                mainGrid[2, 3].inUse = true;
                mainGrid[2, 3].rotateBlock = 1;

                // set shape to 3 to indicate to compiler what shape is being used
                shape = 3;

            }

            // if rand is 4
            if (rand == 4)
            {
                /*
                 * create J block at top of grid
                 * set each block's status to 1 and inuse to true
                 * Create 4 separate integers for rotateblock so it can be used for rotating
                 */
                
                mainGrid[1, 3].status = 1;
                mainGrid[1, 3].inUse = true;
                mainGrid[1, 3].rotateBlock = 4;
                mainGrid[1, 4].status = 1;
                mainGrid[1, 4].inUse = true;
                mainGrid[1, 4].rotateBlock = 3;
                mainGrid[1, 5].status = 1;
                mainGrid[1, 5].inUse = true;
                mainGrid[1, 5].rotateBlock = 2;
                mainGrid[2, 5].status = 1;
                mainGrid[2, 5].inUse = true;
                mainGrid[2, 5].rotateBlock = 1;

                // set shape to 4 to indicate to compiler what shape is being used
                shape = 4;
            }
        }

        // another painting method
        private void Form1_Paint1(object sender, PaintEventArgs e)
        {
            // iterate through rows
            for (int i = 0; i < rows; i++)
            {
                // iterate through columns
                for (int j = 0; j < columns; j++)
                {

                    // if no block is at i, j on main grid
                    if (mainGrid[i, j].status == 0)
                    {

                        // draw rectangle
                        e.Graphics.DrawRectangle(Pens.Red, mainGrid[i, j].rect);
                    }

                    // if block is at i, j on main grid
                    if (mainGrid[i, j].status == 1)
                    {

                        // fill rectangle
                        e.Graphics.FillRectangle(Brushes.Blue, mainGrid[i, j].rect);
                    }

                    // if no block at i, j on buffergrid
                    if (bufferGrid[i, j].status == 0)
                    {

                        // draw rectangle
                        e.Graphics.DrawRectangle(Pens.Red, bufferGrid[i, j].rect);
                    }

                    // if blcok at i, j on buffergrid
                    if (bufferGrid[i, j].status == 1)
                    {

                        // fill rectangle
                        e.Graphics.FillRectangle(Brushes.Blue, bufferGrid[i, j].rect);
                    }
                }
            }
        }

        // painting method
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            // draw rectangle for the border rect variable
            e.Graphics.DrawRectangle(Pens.Black, border);

            // fill rectangle for the background rect variable
            e.Graphics.FillRectangle(Brushes.DarkBlue, background);


            // iterate through rows
            for (int i = 0; i < rows; i++)
            {
                // iterate through columns
                for (int j = 0; j < columns; j++)
                {

                    // if no block at i, j for main grid 
                    if (mainGrid[i, j].status == 0)
                    {

                        // draw rectangle
                        e.Graphics.DrawRectangle(Pens.Red, mainGrid[i, j].rect);
                    }

                    // if block at i, j for main grid
                    if (mainGrid[i, j].status == 1)
                    {

                        // fill rectangle
                        e.Graphics.FillRectangle(Brushes.Blue, mainGrid[i, j].rect);
                    }
                }
            }
        }

        
        // isRowFull method, returns a bool, takes an int as paramater
        public bool isRowFull(int r)
        {
            // iterate through columns
            for (int c = 0; c < columns; c++)
            {

                // if maingrid at r, c has no block or its in use is true
                if (mainGrid[r, c].status == 0 || mainGrid[r, c].inUse == true)
                {

                    // return false
                    return false;
                }
            }

            // increment pts
            pts++;

            // return true
            return true;
        }

        
        //clear row method, no return type, takes int paramater
        public void clearRow(int r)
        {
            // iterate through columns
            for (int c = 0; c < columns; c++)
            {
                mainGrid[r, c].status = 0;
                mainGrid[r, c].inUse = false;
            }
        }

        // row down method, no return value, takes two ints as parameters
        public void rowDown(int r, int counter)
        {
            // iterate through columns
            for (int c = 0; c < columns; c++)
            {

                // set main grid status at r + counter rows down to the same at maingrid at r rows
                mainGrid[r + counter, c].status = mainGrid[r, c].status;

                // set main grid inuse at r + counter rows down to the same at maingrid at r rows 
                mainGrid[r + counter, c].inUse = mainGrid[r, c].inUse;

                // reset main grid status to 0
                mainGrid[r, c].status = 0;
            }
        }

        // clear method, returns int, no parameters
        public int clear()
        {

            // set int rows cleared to 0
            int rowsCleared = 0;

            // iterate through rows
            for (int r = rows - 1; r >= 0; r--)
            {

                // if row is full
                if (isRowFull(r))
                {

                    // clear row
                    clearRow(r);

                    // increment rowsCleared
                    rowsCleared++;
                    
                }


                // else if rows cleared is greater than 0
                else if (rowsCleared > 0)
                {

                    // call rowDown function to move rows down
                    rowDown(r, rowsCleared);
                }
            }

            // toggle paint method
            this.Paint += Form1_Paint1;

            //return rowsCleared
            return rowsCleared;
            
        }

        // reset buffer method, no paramaters, no return types
        public void resetBuffer()
        {
            // iterate through rows
            for (int i = 0; i < rows; i++)
            {

                // iterate through columns
                for (int j = 0; j < columns; j++)
                {

                    // reset buffergrid at i, j inuse to false
                    bufferGrid[i, j].inUse = false;

                    // reset buffergrid at i, j status to 0
                    bufferGrid[i, j].status = 0;

                    // reset buffergrid at i, j rotateblock to 0
                    bufferGrid[i, j].rotateBlock = 0;
                }
            }
        }

        // mainToBuffer method, no parameters, no return value
        public void mainToBuffer()
        {
            // iterate through rows
            for (int r = 0; r < rows; r++)
            {

                // iterate through columns
                for (int c = 0; c < columns; c++)
                {

                    // if block is filled but not being used
                    if (mainGrid[r, c].status == 1 && mainGrid[r, c].inUse == false)
                    {

                        // do nothing
                        counter += 0;

                    }
                    else
                    {

                        // set maingrid at r, c status to buffergrid status at r, c
                        mainGrid[r, c].status = bufferGrid[r, c].status;

                        // set maingrid at r, c inuse to buffergrid inuse at r, c
                        mainGrid[r, c].inUse = bufferGrid[r, c].inUse;

                        // set maingrid at r, c rotateblock to buffergrid rotateblock at r, c
                        mainGrid[r, c].rotateBlock = bufferGrid[r, c].rotateBlock;

                    }
                }
            }

            // toggle paint method
            this.Paint += Form1_Paint1;

            // call reset buffer method
            resetBuffer();

            // iterate through rows
            for (int r = 0; r < rows; r++)
            {

                // iterate through columns
                for (int c = 0; c < columns; c++)
                {

                    // set maingrid and buffergrid at r, c to new rect at its own coordinates and 20x20 dimensions
                    mainGrid[r, c].rect = new Rectangle(mainGrid[r, c].x, mainGrid[r, c].y, 20, 20);
                    bufferGrid[r, c].rect = new Rectangle(bufferGrid[r, c].x, bufferGrid[r, c].y, 20, 20);

                }
            }
        }
    }
}
