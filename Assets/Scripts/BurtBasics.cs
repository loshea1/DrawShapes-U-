using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BurtSharp.UnityInterface;
using UnityEngine.UI;
using BurtSharp.UnityInterface.ClassExtensions;
using System;
using System.IO;
using System.Linq;



public class BurtBasics : MonoBehaviour
{
    private ConnectionSetter    _robot;                             // Access the libraries used for robot control
   // public  Text                messageToUser;
    public  GameObject          cursor;                             // Cursor of the robot's end effector
    //public  GameObject          target;
    //private float               forceMagnitude;                     // Magnitude of the force to be applied on the robot
    //private KeyCode             currentKey;                         // Create a variable that reads the input-key-board
    //private string              forceDirection;                     // String that stores the direction of the force based on arrow-key pressed
    //private float[]             timeStamp = new float[500];         // The time variable to save on Dropbox
    //private Vector3[]           toolPosition = new Vector3[500];    // The tool position to save on Dropbox
    //private int                 indexCount = 0;                     // Keep track of the loop iteration number 
    private float               startTime;                          // Keep track of the time instant at the beginning of the experiment to save the relative time on Dropbox
    //private string              dropboxFolderPath = "/home/robot/Dropbox/BurtBasics/DataCollection/";
    //List<string>                targetX  =  new List<string>();     // Variable that stores the values of the target's X coordinates
    //List<string>                targetY  =  new List<string>();     // ..same for Y
    //List<string>                targetZ  =  new List<string>();     // ..same for Z

    


    
    // Start is called before the first frame update
    void Start()
    {
        _robot  =  GameObject.Find("ConnectionSetter").GetComponent<ConnectionSetter>();                                    // Setup connection with the robot
        cursor  =  GameObject.Find("Cursor");
        //target =  GameObject.Find("Target");
        StartTimer();


        if (_robot != null)
        {
            //_robot.RegisterControlFunction("haptics", CalcForces);
            //_robot.SetActiveControlFunction("haptics");
            _robot.EnableRobot();                                                                                          // Enable the robot - switching from passive resistance mode to transparency mode
            
        }
        else
        {
            BurtSharp.SystemLogger.Error("RobotConnection not found. Try turning on the robot before running the code.");  // Print error if the robot is not found
        }
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateMessage();
        CursorController();
        //CalcForces();
        //SaveFileOnDropbox();
        //readCSVfile();
        //PlotTargets();
        //TargetReached();
    }
/*
    private void UpdateMessage()
    {
        
        if (Input.GetKey(KeyCode.UpArrow))
        {
                forceDirection  =  "FORWARD";
                forceMagnitude  =  5;
                // messageToUser.text = "FORCE " +forceDirection+ " APPLIED WITH MAGNITUDE: " +forceMagnitude+ " N";
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
                forceDirection  =  "BACKWARD";
                forceMagnitude  =  5;
                // messageToUser.text = "FORCE " +forceDirection+ " APPLIED WITH MAGNITUDE: " +forceMagnitude+ " N";
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
                forceDirection  =  "LEFT";
                forceMagnitude  =  5;
                // messageToUser.text = "FORCE " +forceDirection+ " APPLIED WITH MAGNITUDE: " +forceMagnitude+ " N";
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
                forceDirection  =  "RIGHT";
                forceMagnitude  =  5;
                // messageToUser.text = "FORCE " +forceDirection+ " APPLIED WITH MAGNITUDE: " +forceMagnitude+ " N";
        }
        else
        {
                forceMagnitude  =  0;
                messageToUser.text = "Reach the target. Waiting for an arrow-key to be pressed";
        }

    }
    */

    // Locate the cursor aligned with the robot's hand
    void CursorController ()
    {
        //cursor.transform.position  =  new Vector3(_robot.GetToolPosition().x, _robot.GetToolPosition().y, _robot.GetToolPosition().z);  // Read the current position of the robot handle    
        cursor.transform.position  =  new Vector3(_robot.GetToolPosition().x, _robot.GetToolPosition().y, 0f);  // Read the current position of the robot handle    

    }

    // Save the time instant at the beginning of the experiment to create a realtive timer to the experiment duration
    public void StartTimer()
    {
        startTime = Time.time;
    }

/*
    // BurtSharp function to apply forces to robot
    public BurtSharp.CoAP.MsgTypes.RobotCommand CalcForces()
    {
        return RobotCommandExtensions.RobotCommandForce(applyForce(forceMagnitude, forceDirection));
    }


    // Set up the direction of the force based on the arrow-key pressed
    private Vector3 applyForce(float forceMagnitude, string forceDirection) 
    {
        Vector3 output = new Vector3();
        switch (forceDirection)
        {
            case "FORWARD":
                output.Set(0,0,1);  // Forward direction
                break;
            case "BACKWARD":
                output.Set(0,0,-1); // Backward
                break;
            case "LEFT":
                output.Set(-1,0,0); // Left
                break;
            case "RIGHT":
                output.Set(1,0,0); // Right
                break;
        }
        return forceMagnitude * output;
    }

    // Save a .csv file on Dropbox
    void SaveFileOnDropbox()
    {
        if (indexCount < 500)           // Wait for 500 loop iterations to collect data and then save it on file. Not necessary, it is just to avoid to save a big file.
        {
            timeStamp[indexCount]     =  Time.time - startTime;         // Calculate the relative time from the beginning of the experiment
            toolPosition[indexCount]  =  new Vector3(_robot.GetToolPosition().x, _robot.GetToolPosition().y, _robot.GetToolPosition().z);
            indexCount++;
        } 
        else
        {
            string dataString = "X,Y,Z,Time\n"; // Header row
            // Convert Vector3 array to string
            for (int i = 0; i < timeStamp.Length; i++)
            {
                dataString += toolPosition[i].x + "," + toolPosition[i].y + "," + toolPosition[i].z + "," + timeStamp[i] +"\n";
            }

            // Concatenate the strings
            string content = dataString;
            
            // Check if the Dropbox path exist
            if (!Directory.Exists(dropboxFolderPath))
            {
                UnityEngine.Debug.LogError("Dropbox folder path does not exist: " + dropboxFolderPath);
                return;
            }

            // File name and full path
            string fileName = "BurtBasicsExample.txt";
            string fullPath = dropboxFolderPath + fileName;    
            // Check if the file exists
            if (File.Exists(fullPath))
            {
                // Generate a new filename with current date and time
                string dateTimeString   =  DateTime.Now.ToString("ddMMyyyy");
                string newFileName      =  "BurtBasicsExample_" + dateTimeString + ".csv";
                fullPath                =  dropboxFolderPath + newFileName;
            }
            // Save the variable to the file
            SaveToFile(fullPath, content);
        }

    }


    void SaveToFile(string filePath, string content)
    {
        // Write the content to the file
        File.WriteAllText(filePath, content);
    }


    // Read the input file (.csv)
    private void readCSVfile() {

        string filePath        =  "Assets/BurtBasicsInputFile.csv";                 // Input file reading
        string[] lines         =  File.ReadAllLines(filePath);                      // Saves all the lines
        int startMovementLine  =  0;                                                // Skip the header part of the input file
        int count_line         =  0;                                                // Variable to keep track of the line count

        // check if the file exist
        if (File.Exists(filePath)) 
        {
            using(var reader = new StreamReader(filePath))  
		

    		while (!reader.EndOfStream)                                             // Until you don't reach the end of the file, keep reading
			{
				var line    =  reader.ReadLine();                                   // Saving each line
				var values  =  line.Split(',');                                     // Since values are comma-separated, find the comma in order to distinguish each different column
				if (count_line > startMovementLine)                                 // Start saving values only from the correct line on (skipping the header)
				{
                    targetX.Add(values[0]);                                         // Targets
                    targetY.Add(values[1]); 
                    targetZ.Add(values[2]); 
				}
                count_line++;
			}  
        }
            else {
                UnityEngine.Debug.LogError("file not found:" +filePath);            // If the input file is not present in the Asset folder (same folder as this script) or the name/path is different from the one saved on "filePath", then the program cannot read it and returns error
            }
    }

    // Function that plots the targets as a sphere
    void PlotTargets()
    {
        for (int i = 0; i < targetX.Count; i++)
        {
            Vector3 position           =  new Vector3(float.Parse(targetX[0]), float.Parse(targetY[0]), float.Parse(targetZ[0]));
            target.transform.position  =  position;
            // Adjust the size and color of the sphere here if needed
        }
    }


    // Check if the target is reached
    void TargetReached() 
    {
        
            if (_robot.GetToolPosition().x > float.Parse(targetX[0])-0.05 && _robot.GetToolPosition().x < float.Parse(targetX[0])+0.05 && _robot.GetToolPosition().y > float.Parse(targetY[0])-0.05 && _robot.GetToolPosition().y < float.Parse(targetY[0])+0.05 && _robot.GetToolPosition().z > float.Parse(targetZ[0])-0.05 && _robot.GetToolPosition().z < float.Parse(targetZ[0])+0.05)
            {
                messageToUser.text = "TARGET REACHED!!!";
            }
        
        Vector2 toolPos   = new Vector2(_robot.GetToolPosition().x, _robot.GetToolPosition().y);
        Vector2 targetPos = new Vector2(target.transform.position.x, target.transform.position.y);

        float reachThreshold = 0.05f;

        if (Vector2.Distance(toolPos, targetPos) < reachThreshold)
        {
            Debug.Log("TARGET REACHED!!!");
        }
    }
*/
}

