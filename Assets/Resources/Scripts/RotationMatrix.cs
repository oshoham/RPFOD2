using UnityEngine;

/*
 * Lets us rotate by a factor of 90 degrees. Useful for Robots.
 */
public struct RotationMatrix {

	public enum Rotation {
		Identity,
		Left,
		Right,
		Halfway
	}

	public float[] matrix;

	public RotationMatrix(Rotation rot) {
		switch(rot) {
			case Rotation.Identity:
				matrix = new float[4] {1, 0,
						       0, 1};
				break;
			case Rotation.Left:
				matrix = new float[4] {0, -1,
						       1, 0};
				break;
			case Rotation.Right:
				matrix = new float[4] {0, 1,
						       -1, 0};
				break;
			default: // Halfway
				matrix = new float[4] {-1, 0,
						       0, -1};
				break;
		}
	}
	
	/*
	 * Rotate vec by this matrix.
	 */
	public Vector2 Rotate(Vector2 vec) {
		return new Vector2(vec.x * matrix[0] + vec.y * matrix[1],
				   vec.x * matrix[2] + vec.y * matrix[3]);
	}
	
	/*
	 * C# you are more of a pain than you are worth! I need to override this
	 * and GetHashCode for the compiler to stop complaining.
	 */
	public override bool Equals(object o) {
		if(o is RotationMatrix) {
			RotationMatrix r = (RotationMatrix)o;
			return r == this;
		}
		return false;
	}

	public override int GetHashCode() {
		return 0;
	}
	
	public static bool operator !=(RotationMatrix r1, RotationMatrix r2) {
		return !(r1 == r2);
	}
		
	public static bool operator ==(RotationMatrix r1, RotationMatrix r2) {
		for(int i = 0; i < 4; i++) {
			if(r1.matrix[i] != r2.matrix[i])
				return false;
		}
		return true;
	}
}