
using System;

public class FlappyBird
{



		private const float m_vel_loss = 0.03f;
		private const float m_vel_gain = 0.3f;

		//private bool WingUp = false;
		public FlapState CurrentState = FlapState.Down;

		public float Height { get; private set; }

		public float Vel { get; private set; }

		public void evolve (MappedBody body)
		{

				bool wings_up;
				bool flapped = CheckTransition (body, out wings_up);
				if (flapped) {
						//Height += m_height_gain;
						float height_gain = Vel;
						Vel += m_vel_gain;
						Height = Math.Max (Height + Vel, 0);
				} else {
						float height_change = Vel;
						if (Height + height_change <= 0) {
								Vel = Math.Max (0, Vel); //Hit ground: Vel = 0
						}

						Vel -= m_vel_loss;

						if (wings_up) {
								//implement terminal velocity if wings up
								Vel *= 0.7f;

						}

  
                


						Height = Math.Max (0, Height + height_change);
				}
				//Console.WriteLine(string.Format("{0},{1}","Vel: ", Vel.ToString()));
		}

		private bool CheckTransition (MappedBody body, out bool wings_up)
		{
            
            
				FlapState wingPos = FlapDetection.GetFlapState (body);


				wings_up = (wingPos == FlapState.Up);
            

				if (wingPos == FlapState.Up) {
						CurrentState = FlapState.Up;
				} else if (wingPos == FlapState.Middle && CurrentState == FlapState.Up) {
						CurrentState = FlapState.Middle;
				} else if (wingPos == FlapState.Down && CurrentState == FlapState.Middle) {
						CurrentState = FlapState.Down;
						return true;
				}

				//Console.WriteLine(Enum.GetName(typeof(FlapState), wingPos));

				return false;
		}
}






