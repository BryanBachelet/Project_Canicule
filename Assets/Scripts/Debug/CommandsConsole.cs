using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
#if UNITY_EDITOR || DEVELOPMENT_BUILD

namespace BorsalinoTools
{

    /** 
     * Command Console is class allow the editor's user to tap
     * a command to execute it. The idea behind is to help provide
     * debug commands to help developer. 
     * 
     * To use it you need register the command name and create a functiont to
     * subscribe to LogAction event to receive command notification
     */
    public class CommandsConsole : MonoBehaviour
    {
        private string m_inputCommandText;

        // Command window variables
        private bool m_showWindow;
        private bool m_isFirstTimeTrigger;
        private const int m_posX = 20;
        private const int m_posYOffset = 100;
        private const int m_commandWindowWidth = 500;
        private const int m_commandWindowHeight = 100;
        private const int m_commandButtonHeight = 15;
        private const int m_commandTextFieldHeight = 20;
        private const int m_posYTextField = 60;


        // Command window logic variables
        private string m_lastFocusedControl = "";
        private int m_lastTextPosition = 0;
        private int m_lastTextSelectPosition = 0;
        private int m_indexAutoCompletionButton = 4;
        private bool m_hasInputTextChange;
        private const int m_maxAutoCompletionOption = 4;
        private int m_currentArrowInput = 1;

        /** The Player Input variable is important to deactivate player
         *  input while the user type the command
         **/
        private PlayerInput playerInput;

        public delegate void LogAction(string s, params object[] args);
        /* This delegate send a message when a command is call.*/
        public static LogAction OnCommandCall;

        /* Store command name to allow suggestions when typing */
        private static List<string> commandTextList = new List<string>();

        private IDisposable m_EventListener;

        #region Unity Functions       
        public void Awake()
        {
            commandTextList.Clear();
        }

        public void OnEnable()
        {
            m_EventListener = InputSystem.onAnyButtonPress.Call(OnButtonPressed);
        }

        public void OnDisable()
        {
            m_EventListener.Dispose();
        }

        #endregion

        private void OnButtonPressed(InputControl button)
        {
            if (button.displayName == "`")
            {
                OpenCommandWindow();
            }

            if (button.displayName == "Esc")
            {
                CloseCommandWindow();
            }


            m_currentArrowInput = 0;

            if (button.displayName == "Up")
            {
                m_currentArrowInput = 1;
            }

            if (button.displayName == "Down")
            {
                m_currentArrowInput = -1;
            }
        }

        private void OpenCommandWindow()
        {
            if (playerInput == null)
            {
                playerInput = FindFirstObjectByType<PlayerInput>();
            }
            playerInput.DeactivateInput();
            m_showWindow = true;
            m_isFirstTimeTrigger = false;
        }

        private void CloseCommandWindow()
        {
            if (playerInput == null)
            {
                playerInput = FindFirstObjectByType<PlayerInput>();
            }

            playerInput.ActivateInput();
            m_showWindow = false;
            m_inputCommandText = "";
        }

        #region Static Function
        /// <summary>
        /// Functions use to register the command for the autocompletion tools
        /// </summary>
        /// <param name="commandString"></param>
        public static void RegisterCommand(string commandString)
        {
            if (commandTextList.Contains(commandString)) return;

            commandTextList.Add(commandString);
        }

        // Functions to check argument type
        public static int IsInteger(object arg)
        {
            int result = arg is int ? (int)arg : int.MinValue;
            return result;
        }
        public static float IsFloat(object arg)
        {
            float result = arg is float ? (float)arg : float.MinValue;
            return result;
        }
        public static string IsString(object arg)
        {
            string result = arg is string ? (string)arg : null;
            return result;
        }

        #endregion

        #region Window Functions
        private void CreateAutoCompletionButton(string[] preCommand, int buttonCount)
        {
            TextEditor textEditor;

            GUIStyle style = new GUIStyle(GUI.skin.button);
            Color originalBackgroundColor = GUI.backgroundColor;

            GUI.backgroundColor = Color.yellow;

            style.normal.background = MakeBackgroundTexture(10, 10, Color.black);
            style.border = new RectOffset(0, 0, 0, 0);
            style.padding = new RectOffset(0, 0, 0, 0);
            style.margin = new RectOffset(0, 0, 0, 0);

            textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);

            for (int i = 0; i < buttonCount; i++)
            {
                GUI.SetNextControlName(i.ToString());

                if (GUI.Button(new Rect(0, (3 - (buttonCount - 1) + i) * m_commandButtonHeight, m_commandWindowWidth, m_commandButtonHeight), preCommand[i], style))
                {
                    m_inputCommandText = preCommand[i];
                    m_lastTextPosition = preCommand[i].Length;
                    m_lastTextSelectPosition = preCommand[i].Length;
                    GUI.backgroundColor = originalBackgroundColor;
                    return;
                }
            }

            GUI.backgroundColor = originalBackgroundColor;
        }

        private void NavigationCommandWindow(int maxOptions)
        {
            if (!m_showWindow) return;
            TextEditor textEditor;

            if ((m_currentArrowInput == -1))
            {
                m_indexAutoCompletionButton++;
                if (m_indexAutoCompletionButton > maxOptions - 1)
                {
                    m_indexAutoCompletionButton = Mathf.Clamp(m_indexAutoCompletionButton, 0, maxOptions - 1);
                    GUI.FocusControl("CommandArea");
                    textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                    m_lastTextPosition = m_inputCommandText.Length;
                    m_lastTextSelectPosition = m_inputCommandText.Length;
                }
                else
                {
                    m_indexAutoCompletionButton = Mathf.Clamp(m_indexAutoCompletionButton, 0, maxOptions - 1);
                    GUI.FocusControl(m_indexAutoCompletionButton.ToString());
                }
            }


            if ((m_currentArrowInput == 1))
            {
                m_indexAutoCompletionButton--;
                m_indexAutoCompletionButton = Mathf.Clamp(m_indexAutoCompletionButton, 0, maxOptions - 1);
                GUI.FocusControl(m_indexAutoCompletionButton.ToString());
            }

            m_currentArrowInput = 0;

        }

        private void ManageTextField()
        {
            TextEditor textEditor;
            string text = GUI.TextField(new Rect(0, m_posYTextField, m_commandWindowWidth, m_commandTextFieldHeight), m_inputCommandText);
            m_inputCommandText = text;
            m_hasInputTextChange = GUI.changed;

            if (!m_isFirstTimeTrigger)
            {
                m_isFirstTimeTrigger = true;

                GUI.FocusControl("CommandArea");
            }
            if (GUI.GetNameOfFocusedControl() == "CommandArea")
            {
                textEditor = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
                if (GUI.GetNameOfFocusedControl() != m_lastFocusedControl)
                {
                    textEditor.cursorIndex = m_lastTextPosition;
                    textEditor.selectIndex = m_lastTextSelectPosition;
                }
                else
                {
                    m_lastTextPosition = textEditor.cursorIndex;
                    m_lastTextSelectPosition = textEditor.selectIndex;
                }
            }


        }

        public void OnGUI()
        {
            if (m_showWindow)
            {
                // Searching for corresponding commands contaning the prefix 
                string[] preCommand = FindPrefixCommand(m_inputCommandText);
                int maxPreCommandOption = preCommand != null ? preCommand.Length : 0;
                int maxAutoCompletionOptions = Mathf.Min(m_maxAutoCompletionOption, maxPreCommandOption);

                GUILayout.BeginArea(new Rect(m_posX, Screen.height - m_posYOffset, m_commandWindowWidth, m_commandWindowHeight));

                if (preCommand != null && preCommand.Length != 0)
                {
                    CreateAutoCompletionButton(preCommand, maxAutoCompletionOptions);
                }

                GUI.SetNextControlName("CommandArea");
                if (m_hasInputTextChange && GUI.GetNameOfFocusedControl() != "CommandArea" || GUI.GetNameOfFocusedControl() == "")
                {
                    GUI.FocusControl("CommandArea");
                }

                NavigationCommandWindow(maxAutoCompletionOptions);
                ManageTextField();
                ComputeCommand();

                 m_lastFocusedControl = GUI.GetNameOfFocusedControl();


                GUILayout.EndArea();
            }
        }

        public string[] FindPrefixCommand(string prefix)
        {
            if (prefix == null)
                return null;


            CompareInfo myComp = CultureInfo.InvariantCulture.CompareInfo;
            List<string> validCommand = new List<string>();
            for (int i = 0; i < commandTextList.Count; i++)
            {
                if (myComp.IsPrefix(commandTextList[i], prefix))
                {
                    validCommand.Add(commandTextList[i]);
                }
            }

            return validCommand.ToArray();
        }

        // Functions to recreate texture 
        private Texture2D MakeBackgroundTexture(int width, int height, Color color)
        {
            Color[] pixels = new Color[width * height];

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            Texture2D backgroundTexture = new Texture2D(width, height);

            backgroundTexture.SetPixels(pixels);
            backgroundTexture.Apply();

            return backgroundTexture;
        }
        public void ComputeCommand()
        {
            if (!m_showWindow)
                return;

            if ((Event.current.keyCode == KeyCode.Return) && m_inputCommandText != "" && GUI.GetNameOfFocusedControl() == "CommandArea")
            {
                if (Event.current.type != EventType.Used)
                    return;

                m_inputCommandText = m_inputCommandText.Trim();
                string[] instruction = m_inputCommandText.Split(" ");
                object[] parameters = new object[instruction.Length - 1];
                int countValidParameter = 0;
                for (int i = 1; i < instruction.Length; i++)
                {
                    float valFloat = 0;
                    int val = 0;
                    bool isFloat = float.TryParse(instruction[i], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out valFloat);
                    if (instruction[i] == "")
                    {
                        continue;
                    }
                    else if ((instruction[i].Contains('.') || instruction[i].Contains(',')) && isFloat)
                    {
                        parameters[countValidParameter] = valFloat;
                        countValidParameter++;
                    }
                    else if (int.TryParse(instruction[i], out val))
                    {
                        parameters[countValidParameter] = val;
                        countValidParameter++;
                    }
                    else
                    {
                        parameters[countValidParameter] = instruction[i];
                        countValidParameter++;
                    }
                }
                object[] finalParameters = new object[countValidParameter];
                System.Array.Copy(parameters, finalParameters, countValidParameter);

                Debug.Log("Command : " + m_inputCommandText);
                OnCommandCall?.Invoke(instruction[0], finalParameters);
                m_inputCommandText = "";
            }
        }

      

        #endregion
#endif
    }
}