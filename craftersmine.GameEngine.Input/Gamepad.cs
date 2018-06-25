using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.Utils;
using XInput.Wrapper;

namespace craftersmine.GameEngine.Input
{
    /// <summary>
    /// Allows to use gamepad input
    /// </summary>
    public sealed class Gamepad
    {
        private static X.Gamepad fstPlayer { get; set; } = X.Gamepad_1;
        private static X.Gamepad secPlayer { get; set; } = X.Gamepad_2;
        private static X.Gamepad trdPlayer { get; set; } = X.Gamepad_3;
        private static X.Gamepad fthPlayer { get; set; } = X.Gamepad_4;

        #region Fields
        private static Buttons buttonsFst = new Buttons();
        private static Buttons buttonsSec = new Buttons();
        private static Buttons buttonsTrd = new Buttons();
        private static Buttons buttonsFth = new Buttons();

        private static DPad dPadFst = new DPad();
        private static DPad dPadSec = new DPad();
        private static DPad dPadTrd = new DPad();
        private static DPad dPadFth = new DPad();

        private static Sticks sticksFst = new Sticks();
        private static Sticks sticksSec = new Sticks();
        private static Sticks sticksTrd = new Sticks();
        private static Sticks sticksFth = new Sticks();

        private static Triggers trigsFst = new Triggers();
        private static Triggers trigsSec = new Triggers();
        private static Triggers trigsTrd = new Triggers();
        private static Triggers trigsFth = new Triggers();

        private static BatteryInfo batFst = new BatteryInfo();
        private static BatteryInfo batSec = new BatteryInfo();
        private static BatteryInfo batTrd = new BatteryInfo();
        private static BatteryInfo batFth = new BatteryInfo();
        #endregion

        /// <summary>
        /// Returns states of buttons from <paramref name="playerNumber"/> gamepad
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <returns><see cref="Buttons"/> information</returns>
        public static Buttons GetButtons(Player playerNumber)
        {
            if (X.IsAvailable)
            {
                switch (playerNumber)
                {
                    case Player.First:
                        if (fstPlayer.Update())
                            if (fstPlayer.IsConnected)
                            {
                                buttonsFst.A = fstPlayer.A_down;
                                buttonsFst.B = fstPlayer.B_down;
                                buttonsFst.X = fstPlayer.X_down;
                                buttonsFst.Y = fstPlayer.Y_down;
                                buttonsFst.LB = fstPlayer.LBumper_down;
                                buttonsFst.RB = fstPlayer.RBumper_down;
                                buttonsFst.LeftStick = fstPlayer.LStick_down;
                                buttonsFst.RightStick = fstPlayer.RStick_down;
                                buttonsFst.Back = fstPlayer.Back_down;
                                buttonsFst.Start = fstPlayer.Start_down;
                            }
                        break;
                    case Player.Second:
                        if (secPlayer.Update())
                            if (secPlayer.IsConnected)
                            {
                                buttonsSec.A = secPlayer.A_down;
                                buttonsSec.B = secPlayer.B_down;
                                buttonsSec.X = secPlayer.X_down;
                                buttonsSec.Y = secPlayer.Y_down;
                                buttonsSec.LB = secPlayer.LBumper_down;
                                buttonsSec.RB = secPlayer.RBumper_down;
                                buttonsSec.LeftStick = secPlayer.LStick_down;
                                buttonsSec.RightStick = secPlayer.RStick_down;
                                buttonsSec.Back = secPlayer.Back_down;
                                buttonsSec.Start = secPlayer.Start_down;
                            }
                        break;
                    case Player.Third:
                        if (trdPlayer.Update())
                            if (trdPlayer.IsConnected)
                            {
                                buttonsTrd.A = trdPlayer.A_down;
                                buttonsTrd.B = trdPlayer.B_down;
                                buttonsTrd.X = trdPlayer.X_down;
                                buttonsTrd.Y = trdPlayer.Y_down;
                                buttonsTrd.LB = trdPlayer.LBumper_down;
                                buttonsTrd.RB = trdPlayer.RBumper_down;
                                buttonsTrd.LeftStick = trdPlayer.LStick_down;
                                buttonsTrd.RightStick = trdPlayer.RStick_down;
                                buttonsTrd.Back = trdPlayer.Back_down;
                                buttonsTrd.Start = trdPlayer.Start_down;
                            }
                        break;
                    case Player.Fourth:
                        if (fthPlayer.Update())
                            if (fthPlayer.IsConnected)
                            {
                                buttonsFth.A = fthPlayer.A_down;
                                buttonsFth.B = fthPlayer.B_down;
                                buttonsFth.X = fthPlayer.X_down;
                                buttonsFth.Y = fthPlayer.Y_down;
                                buttonsFth.LB = fthPlayer.LBumper_down;
                                buttonsFth.RB = fthPlayer.RBumper_down;
                                buttonsFth.LeftStick = fthPlayer.LStick_down;
                                buttonsFth.RightStick = fthPlayer.RStick_down;
                                buttonsFth.Back = fthPlayer.Back_down;
                                buttonsFth.Start = fthPlayer.Start_down;
                            }
                        break;
                }
            }
            switch (playerNumber)
            {
                case Player.First: return buttonsFst;
                case Player.Second: return buttonsSec;
                case Player.Third: return buttonsTrd;
                case Player.Fourth: return buttonsFth;
                default: return buttonsFst;
            }
        }

        /// <summary>
        /// Returns states of DPad buttons from <paramref name="playerNumber"/> gamepad
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <returns><see cref="DPad"/> information</returns>
        public static DPad GetDPad(Player playerNumber)
        {
            if (X.IsAvailable)
            {
                switch (playerNumber)
                {
                    case Player.First:
                            if (fstPlayer.IsConnected)
                            {
                                dPadFst.Up = fstPlayer.Dpad_Up_down;
                                dPadFst.Down = fstPlayer.Dpad_Down_down;
                                dPadFst.Left = fstPlayer.Dpad_Left_down;
                                dPadFst.Right = fstPlayer.Dpad_Right_down;
                            }
                        break;
                    case Player.Second:
                            if (secPlayer.IsConnected)
                            {
                                dPadSec.Up = secPlayer.Dpad_Up_down;
                                dPadSec.Down = secPlayer.Dpad_Down_down;
                                dPadSec.Left = secPlayer.Dpad_Left_down;
                                dPadSec.Right = secPlayer.Dpad_Right_down;
                            }
                        break;
                    case Player.Third:
                            if (trdPlayer.IsConnected)
                            {
                                dPadTrd.Up = trdPlayer.Dpad_Up_down;
                                dPadTrd.Down = trdPlayer.Dpad_Down_down;
                                dPadTrd.Left = trdPlayer.Dpad_Left_down;
                                dPadTrd.Right = trdPlayer.Dpad_Right_down;
                            }
                        break;
                    case Player.Fourth:
                            if (fthPlayer.IsConnected)
                            {
                                dPadFth.Up = fthPlayer.Dpad_Up_down;
                                dPadFth.Down = fthPlayer.Dpad_Down_down;
                                dPadFth.Left = fthPlayer.Dpad_Left_down;
                                dPadFth.Right = fthPlayer.Dpad_Right_down;
                            }
                        break;
                }
            }
            switch (playerNumber)
            {
                case Player.First: return dPadFst;
                case Player.Second: return dPadSec;
                case Player.Third: return dPadTrd;
                case Player.Fourth: return dPadFth;
                default: return dPadFst;
            }
        }

        /// <summary>
        /// Returns positions of sticks axis from <paramref name="playerNumber"/> gamepad
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <returns><see cref="Sticks"/> information</returns>
        public static Sticks GetSticks(Player playerNumber)
        {
            if (X.IsAvailable)
            {
                switch (playerNumber)
                {
                    case Player.First:
                            if (fstPlayer.IsConnected)
                            {
                                sticksFst.LeftStickAxisX = fstPlayer.LStick_N.X;
                                sticksFst.LeftStickAxisY = fstPlayer.LStick_N.Y;
                                sticksFst.RightStickAxisX = fstPlayer.RStick_N.X;
                                sticksFst.RightStickAxisY = fstPlayer.RStick_N.Y;
                                sticksFst.LeftStickDeadZone = fstPlayer.LStick_DeadZone;
                                sticksFst.RightStickDeadZone = fstPlayer.RStick_DeadZone;
                            }
                        break;
                    case Player.Second:
                            if (secPlayer.IsConnected)
                            {
                                sticksSec.LeftStickAxisX = secPlayer.LStick_N.X;
                                sticksSec.LeftStickAxisY = secPlayer.LStick_N.Y;
                                sticksSec.RightStickAxisX = secPlayer.RStick_N.X;
                                sticksSec.RightStickAxisY = secPlayer.RStick_N.Y;
                                sticksSec.LeftStickDeadZone = secPlayer.LStick_DeadZone;
                                sticksSec.RightStickDeadZone = secPlayer.RStick_DeadZone;
                            }
                        break;
                    case Player.Third:
                            if (trdPlayer.IsConnected)
                            {
                                sticksTrd.LeftStickAxisX = trdPlayer.LStick_N.X;
                                sticksTrd.LeftStickAxisY = trdPlayer.LStick_N.Y;
                                sticksTrd.RightStickAxisX = trdPlayer.RStick_N.X;
                                sticksTrd.RightStickAxisY = trdPlayer.RStick_N.Y;
                                sticksTrd.LeftStickDeadZone = trdPlayer.LStick_DeadZone;
                                sticksTrd.RightStickDeadZone = trdPlayer.RStick_DeadZone;
                            }
                        break;
                    case Player.Fourth:
                            if (fthPlayer.IsConnected)
                            {
                                sticksFth.LeftStickAxisX = fthPlayer.LStick_N.X;
                                sticksFth.LeftStickAxisY = fthPlayer.LStick_N.Y;
                                sticksFth.RightStickAxisX = fthPlayer.RStick_N.X;
                                sticksFth.RightStickAxisY = fthPlayer.RStick_N.Y;
                                sticksFth.LeftStickDeadZone = fthPlayer.LStick_DeadZone;
                                sticksFth.RightStickDeadZone = fthPlayer.RStick_DeadZone;
                            }
                        break;
                }
            }
            switch (playerNumber)
            {
                case Player.First: return sticksFst;
                case Player.Second: return sticksSec;
                case Player.Third: return sticksTrd;
                case Player.Fourth: return sticksFth;
                default: return sticksFst;
            }
        }
        
        /// <summary>
        /// Returns levels of triggers from <paramref name="playerNumber"/> gamepad
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <returns><see cref="Triggers"/> information</returns>
        public static Triggers GetTriggers(Player playerNumber)
        {
            if (X.IsAvailable)
            {
                switch (playerNumber)
                {
                    case Player.First:
                            if (fstPlayer.IsConnected)
                            {
                                trigsFst.LT = fstPlayer.LTrigger_N;
                                trigsFst.RT = fstPlayer.RTrigger_N;
                                trigsFst.LTThreshold = fstPlayer.LTrigger_Threshold;
                                trigsFst.RTThreshold = fstPlayer.RTrigger_Threshold;
                            }
                        break;
                    case Player.Second:
                            if (secPlayer.IsConnected)
                            {
                                trigsSec.LT = secPlayer.LTrigger_N;
                                trigsSec.RT = secPlayer.RTrigger_N;
                                trigsSec.LTThreshold = secPlayer.LTrigger_Threshold;
                                trigsSec.RTThreshold = secPlayer.RTrigger_Threshold;
                            }
                        break;
                    case Player.Third:
                            if (trdPlayer.IsConnected)
                            {
                                trigsTrd.LT = trdPlayer.LTrigger_N;
                                trigsTrd.RT = trdPlayer.RTrigger_N;
                                trigsTrd.LTThreshold = trdPlayer.LTrigger_Threshold;
                                trigsTrd.RTThreshold = trdPlayer.RTrigger_Threshold;
                            }
                        break;
                    case Player.Fourth:
                            if (fthPlayer.IsConnected)
                            {
                                trigsFth.LT = fthPlayer.LTrigger_N;
                                trigsFth.RT = fthPlayer.RTrigger_N;
                                trigsFth.LTThreshold = fthPlayer.LTrigger_Threshold;
                                trigsFth.RTThreshold = fthPlayer.RTrigger_Threshold;
                            }
                        break;
                }
            }
            switch (playerNumber)
            {
                case Player.First: return trigsFst;
                case Player.Second: return trigsSec;
                case Player.Third: return trigsTrd;
                case Player.Fourth: return trigsFth;
                default: return trigsFst;
            }
        }

        /// <summary>
        /// Returns battery information of <paramref name="playerNumber"/> gamepad
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <returns><see cref="BatteryInfo"/> information</returns>
        public static BatteryInfo GetBatteryInfo(Player playerNumber)
        {
            switch (playerNumber)
            {
                case Player.First:
                    switch (fstPlayer.BatteryInfo.ChargeLevel)
                    {
                        case X.Gamepad.Battery.ChargeLevel.Empty:
                        default:
                            batFst.Charge = BatteryChargeLevel.Empty;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Low:
                            batFst.Charge = BatteryChargeLevel.Low;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Medium:
                            batFst.Charge = BatteryChargeLevel.Medium;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Full:
                            batFst.Charge = BatteryChargeLevel.High;
                            break;
                    }
                    switch (fstPlayer.BatteryInfo.BatteryType)
                    {
                        case X.Gamepad.Battery.Types.Alkaline:
                            batFst.IsConnected = true;
                            batFst.BatteryType = BatteryType.Alkaline;
                            break;
                        case X.Gamepad.Battery.Types.NiMh:
                            batFst.IsConnected = true;
                            batFst.BatteryType = BatteryType.Rechargable;
                            break;
                        case X.Gamepad.Battery.Types.Wired:
                            batFst.IsWiredConnection = true;
                            batFst.BatteryType = BatteryType.Wired;
                            break;
                        case X.Gamepad.Battery.Types.Unknown:
                            batFst.IsConnected = true;
                            batFst.BatteryType = BatteryType.Unknown;
                            break;
                        case X.Gamepad.Battery.Types.Disconnected:
                            batFst.IsConnected = false;
                            batFst.BatteryType = BatteryType.NoBattery;
                            break;
                    }
                    break;
                case Player.Second:
                    switch (secPlayer.BatteryInfo.ChargeLevel)
                    {
                        case X.Gamepad.Battery.ChargeLevel.Empty:
                        default:
                            batSec.Charge = BatteryChargeLevel.Empty;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Low:
                            batSec.Charge = BatteryChargeLevel.Low;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Medium:
                            batSec.Charge = BatteryChargeLevel.Medium;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Full:
                            batSec.Charge = BatteryChargeLevel.High;
                            break;
                    }
                    switch (secPlayer.BatteryInfo.BatteryType)
                    {
                        case X.Gamepad.Battery.Types.Alkaline:
                            batSec.IsConnected = true;
                            batSec.BatteryType = BatteryType.Alkaline;
                            break;
                        case X.Gamepad.Battery.Types.NiMh:
                            batSec.IsConnected = true;
                            batSec.BatteryType = BatteryType.Rechargable;
                            break;
                        case X.Gamepad.Battery.Types.Wired:
                            batSec.IsWiredConnection = true;
                            batSec.BatteryType = BatteryType.Wired;
                            break;
                        case X.Gamepad.Battery.Types.Unknown:
                            batSec.IsConnected = true;
                            batSec.BatteryType = BatteryType.Unknown;
                            break;
                        case X.Gamepad.Battery.Types.Disconnected:
                            batSec.IsConnected = false;
                            batSec.BatteryType = BatteryType.NoBattery;
                            break;
                    }
                    break;
                case Player.Third:
                    switch (trdPlayer.BatteryInfo.ChargeLevel)
                    {
                        case X.Gamepad.Battery.ChargeLevel.Empty:
                        default:
                            batTrd.Charge = BatteryChargeLevel.Empty;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Low:
                            batTrd.Charge = BatteryChargeLevel.Low;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Medium:
                            batTrd.Charge = BatteryChargeLevel.Medium;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Full:
                            batTrd.Charge = BatteryChargeLevel.High;
                            break;
                    }
                    switch (trdPlayer.BatteryInfo.BatteryType)
                    {
                        case X.Gamepad.Battery.Types.Alkaline:
                            batTrd.IsConnected = true;
                            batTrd.BatteryType = BatteryType.Alkaline;
                            break;
                        case X.Gamepad.Battery.Types.NiMh:
                            batTrd.IsConnected = true;
                            batTrd.BatteryType = BatteryType.Rechargable;
                            break;
                        case X.Gamepad.Battery.Types.Wired:
                            batTrd.IsWiredConnection = true;
                            batTrd.BatteryType = BatteryType.Wired;
                            break;
                        case X.Gamepad.Battery.Types.Unknown:
                            batTrd.IsConnected = true;
                            batTrd.BatteryType = BatteryType.Unknown;
                            break;
                        case X.Gamepad.Battery.Types.Disconnected:
                            batTrd.IsConnected = false;
                            batTrd.BatteryType = BatteryType.NoBattery;
                            break;
                    }
                    break;
                case Player.Fourth:
                    switch (fthPlayer.BatteryInfo.ChargeLevel)
                    {
                        case X.Gamepad.Battery.ChargeLevel.Empty:
                        default:
                            batFth.Charge = BatteryChargeLevel.Empty;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Low:
                            batFth.Charge = BatteryChargeLevel.Low;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Medium:
                            batFth.Charge = BatteryChargeLevel.Medium;
                            break;
                        case X.Gamepad.Battery.ChargeLevel.Full:
                            batFth.Charge = BatteryChargeLevel.High;
                            break;
                    }
                    switch (fthPlayer.BatteryInfo.BatteryType)
                    {
                        case X.Gamepad.Battery.Types.Alkaline:
                            batFth.IsConnected = true;
                            batFth.BatteryType = BatteryType.Alkaline;
                            break;
                        case X.Gamepad.Battery.Types.NiMh:
                            batFth.IsConnected = true;
                            batFth.BatteryType = BatteryType.Rechargable;
                            break;
                        case X.Gamepad.Battery.Types.Wired:
                            batFth.IsWiredConnection = true;
                            batFth.BatteryType = BatteryType.Wired;
                            break;
                        case X.Gamepad.Battery.Types.Unknown:
                            batFth.IsConnected = true;
                            batFth.BatteryType = BatteryType.Unknown;
                            break;
                        case X.Gamepad.Battery.Types.Disconnected:
                            batFth.IsConnected = false;
                            batFth.BatteryType = BatteryType.NoBattery;
                            break;
                    }
                    break;
            }
            switch (playerNumber)
            {
                case Player.First:
                default: return batFst;
                case Player.Second: return batSec;
                case Player.Third: return batTrd;
                case Player.Fourth: return batFth;
            }
        }

        /// <summary>
        /// Starts vibration of <paramref name="motors"/> at <paramref name="playerNumber"/> gamepad with <paramref name="strength"/> and for <paramref name="duration"/>
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <param name="strength">Strength of vibration</param>
        /// <param name="duration">Duration of vibration</param>
        /// <param name="motors">Vibrating motors</param>
        public static void Vibrate(Player playerNumber, float strength, int duration, VibrateMotors motors)
        {
            switch (playerNumber)
            {
                case Player.First:
                    switch (motors)
                    {
                        case VibrateMotors.HiFrequency:
                            fstPlayer.FFB_RightMotor(strength, duration);
                            break;
                        case VibrateMotors.LowFrequency:
                            fstPlayer.FFB_LeftMotor(strength, duration);
                            break;
                        case VibrateMotors.Both:
                        default:
                            fstPlayer.FFB_Vibrate(strength, strength, duration);
                            break;
                    }
                    break;
                case Player.Second:
                    switch (motors)
                    {
                        case VibrateMotors.HiFrequency:
                            secPlayer.FFB_RightMotor(strength, duration);
                            break;
                        case VibrateMotors.LowFrequency:
                            secPlayer.FFB_LeftMotor(strength, duration);
                            break;
                        case VibrateMotors.Both:
                        default:
                            secPlayer.FFB_Vibrate(strength, strength, duration);
                            break;
                    }
                    break;
                case Player.Third:
                    switch (motors)
                    {
                        case VibrateMotors.HiFrequency:
                            trdPlayer.FFB_RightMotor(strength, duration);
                            break;
                        case VibrateMotors.LowFrequency:
                            trdPlayer.FFB_LeftMotor(strength, duration);
                            break;
                        case VibrateMotors.Both:
                        default:
                            trdPlayer.FFB_Vibrate(strength, strength, duration);
                            break;
                    }
                    break;
                case Player.Fourth:
                    switch (motors)
                    {
                        case VibrateMotors.HiFrequency:
                            fthPlayer.FFB_RightMotor(strength, duration);
                            break;
                        case VibrateMotors.LowFrequency:
                            fthPlayer.FFB_LeftMotor(strength, duration);
                            break;
                        case VibrateMotors.Both:
                        default:
                            fthPlayer.FFB_Vibrate(strength, strength, duration);
                            break;
                    }
                    break;
            }
        }

        public const int ThumbstickMaxValue = 32767;
        //public const int ThumbstickMinValue = -32767;
        public const int TriggerMaxValue = 255;
        //public const int TriggerMinValue = -255;

        /// <summary>
        /// Sets dead zone of <paramref name="deadZoneControl"/> control in range 0.0f - 1.0f of <paramref name="playerNumber"/>
        /// </summary>
        /// <param name="playerNumber">Number of gamepad</param>
        /// <param name="deadZoneControl">Gamepad control which changing deadzone</param>
        /// <param name="deadzone">Deadzone value in range 0.0f - 1.0f</param>
        public static void SetDeadzone(Player playerNumber, DeadZoneControl deadZoneControl, float deadzone)
        {
            switch (playerNumber)
            {
                case Player.First:
                    switch (deadZoneControl)
                    {
                        case DeadZoneControl.LeftThumbstick:
                            int deadZoneLS = ((int)(ThumbstickMaxValue * deadzone));
                            fstPlayer.LStick_DeadZone = deadZoneLS;
                            break;
                        case DeadZoneControl.RightThumbstick:
                            int deadZoneRS = ((int)(ThumbstickMaxValue * deadzone));
                            fstPlayer.RStick_DeadZone = deadZoneRS;
                            break;
                        case DeadZoneControl.LeftTrigger:
                            int deadZoneLT = ((int)(TriggerMaxValue * deadzone));
                            fstPlayer.LTrigger_Threshold = deadZoneLT;
                            break;
                        case DeadZoneControl.RightTrigger:
                            int deadZoneRT = ((int)(TriggerMaxValue * deadzone));
                            fstPlayer.RTrigger_Threshold = deadZoneRT;
                            break;
                    }
                    break;
                case Player.Second:
                    switch (deadZoneControl)
                    {
                        case DeadZoneControl.LeftThumbstick:
                            int deadZoneLS = ((int)(ThumbstickMaxValue * deadzone));
                            secPlayer.LStick_DeadZone = deadZoneLS;
                            break;
                        case DeadZoneControl.RightThumbstick:
                            int deadZoneRS = ((int)(ThumbstickMaxValue * deadzone));
                            secPlayer.RStick_DeadZone = deadZoneRS;
                            break;
                        case DeadZoneControl.LeftTrigger:
                            int deadZoneLT = ((int)(TriggerMaxValue * deadzone));
                            secPlayer.LTrigger_Threshold = deadZoneLT;
                            break;
                        case DeadZoneControl.RightTrigger:
                            int deadZoneRT = ((int)(TriggerMaxValue * deadzone));
                            secPlayer.RTrigger_Threshold = deadZoneRT;
                            break;
                    }
                    break;
                case Player.Third:
                    switch (deadZoneControl)
                    {
                        case DeadZoneControl.LeftThumbstick:
                            int deadZoneLS = ((int)(ThumbstickMaxValue * deadzone));
                            trdPlayer.LStick_DeadZone = deadZoneLS;
                            break;
                        case DeadZoneControl.RightThumbstick:
                            int deadZoneRS = ((int)(ThumbstickMaxValue * deadzone));
                            trdPlayer.RStick_DeadZone = deadZoneRS;
                            break;
                        case DeadZoneControl.LeftTrigger:
                            int deadZoneLT = ((int)(TriggerMaxValue * deadzone));
                            trdPlayer.LTrigger_Threshold = deadZoneLT;
                            break;
                        case DeadZoneControl.RightTrigger:
                            int deadZoneRT = ((int)(TriggerMaxValue * deadzone));
                            trdPlayer.RTrigger_Threshold = deadZoneRT;
                            break;
                    }
                    break;
                case Player.Fourth:
                    switch (deadZoneControl)
                    {
                        case DeadZoneControl.LeftThumbstick:
                            int deadZoneLS = ((int)(ThumbstickMaxValue * deadzone));
                            fthPlayer.LStick_DeadZone = deadZoneLS;
                            break;
                        case DeadZoneControl.RightThumbstick:
                            int deadZoneRS = ((int)(ThumbstickMaxValue * deadzone));
                            fthPlayer.RStick_DeadZone = deadZoneRS;
                            break;
                        case DeadZoneControl.LeftTrigger:
                            int deadZoneLT = ((int)(TriggerMaxValue * deadzone));
                            fthPlayer.LTrigger_Threshold = deadZoneLT;
                            break;
                        case DeadZoneControl.RightTrigger:
                            int deadZoneRT = ((int)(TriggerMaxValue * deadzone));
                            fthPlayer.RTrigger_Threshold = deadZoneRT;
                            break;
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Number of player gamepad
    /// </summary>
    public enum Player
    {
        First, Second, Third, Fourth
    }

    public enum DeadZoneControl
    {
        LeftThumbstick, RightThumbstick, LeftTrigger, RightTrigger
    }

    /// <summary>
    /// Vibrating motors
    /// </summary>
    public enum VibrateMotors
    {
        Both, HiFrequency, LowFrequency
    }

    /// <summary>
    /// Level of battery charge
    /// </summary>
    public enum BatteryChargeLevel
    {
        Low, Medium, High, Empty
    }

    /// <summary>
    /// Battery type
    /// </summary>
    public enum BatteryType
    {
        NoBattery, Rechargable, Alkaline, Wired, Unknown
    }

    /// <summary>
    /// Gamepad informaion of pressed buttons
    /// </summary>
    public struct Buttons
    {
        [DefaultValue(false)]
        public bool A { get; internal set; }
        [DefaultValue(false)]
        public bool B { get; internal set; }
        [DefaultValue(false)]
        public bool X { get; internal set; }
        [DefaultValue(false)]
        public bool Y { get; internal set; }

        [DefaultValue(false)]
        public bool LeftStick { get; internal set; }
        [DefaultValue(false)]
        public bool RightStick { get; internal set; }

        [DefaultValue(false)]
        public bool LB { get; internal set; }
        [DefaultValue(false)]
        public bool RB { get; internal set; }

        [DefaultValue(false)]
        public bool Start { get; internal set; }
        [DefaultValue(false)]
        public bool Back { get; internal set; }
        public bool ChangeView { get { return Back; } internal set { Back = value; } }
        public bool Menu { get { return Start; } internal set { Start = value; } }
    }

    /// <summary>
    /// Gamepad information of pressed DPad buttons
    /// </summary>
    public struct DPad
    {
        [DefaultValue(false)]
        public bool Up { get; internal set; }
        [DefaultValue(false)]
        public bool Left { get; internal set; }
        [DefaultValue(false)]
        public bool Down { get; internal set; }
        [DefaultValue(false)]
        public bool Right { get; internal set; }
    }

    /// <summary>
    /// Gamepad informaiton of thumbsticks axis
    /// </summary>
    public struct Sticks
    {
        public float LeftStickAxisX { get; internal set; }
        public float LeftStickAxisY { get; internal set; }
        
        public float RightStickAxisX { get; internal set; }
        public float RightStickAxisY { get; internal set; }

        public float LeftStickDeadZone { get; internal set; }
        public float RightStickDeadZone { get; internal set; }
    }

    /// <summary>
    /// Gamepad information of trigger levels
    /// </summary>
    public struct Triggers
    {
        public float LT { get; internal set; }
        public float RT { get; internal set; }

        public float LTThreshold { get; internal set; }
        public float RTThreshold { get; internal set; }
    }

    /// <summary>
    /// Gamepad information of battery
    /// </summary>
    public struct BatteryInfo
    {
        public BatteryChargeLevel Charge { get; internal set; }
        public BatteryType BatteryType { get; internal set; }
        public bool IsWiredConnection { get; internal set; }
        public bool IsConnected { get; internal set; }
    }
}
