using System;
using UnityEngine;
using System.Collections;

public class _MouseInput : MonoBehaviour {
	public GameObject[] objs; // Objectos a Manipular

	private ManyMouse[] _manyMouseMice; 	// Arreglo de Mouses
	private Vector3[] move; 			   // Vector a ser Aplicado a los objetos
	private Vector2[] moveUI; 			  // Vector a Mostrar en Interfaz

	private float actualDistance; // Distancia de la camara a los objetos
	public float sens = 0.05f;   // Sensibilidad de Mouse

	void Start() { // Inicializacion
		// Bloquear movimientos del mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);

		// Obtener Mouses conectados
	    int numMice = ManyMouseWrapper.MouseCount;
        if (numMice > 0) {
        	// Inicializar arreglos en base a nuemro de mouses
            _manyMouseMice = new ManyMouse[numMice];
            move = new Vector3[numMice];
			moveUI = new Vector2[numMice];
			// Obtener IDs de Mouses
            for (int i = 0; i < numMice; i++) {
                _manyMouseMice[i] = ManyMouseWrapper.GetMouseByID(i);
                //_manyMouseMice[i].EventButtonDown += UpdateButtons;
            }
        }

        // Obtener Distancia de Camara
		Vector3 toObjectVector = transform.position - Camera.main.transform.position;
		Vector3 linearDistanceVector = Vector3.Project(toObjectVector,Camera.main.transform.forward);
		actualDistance = linearDistanceVector.magnitude;
	} // Start
/*
	private void UpdateButtons(ManyMouse mouse, int buttonId){
		_manyMouseMice.Buttons[]
	}

	String ButtontoString(){

		return ""+
	}
*/
	void Update() {
		// Re-Bloquear Mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = (false);

		// Itera sobre todos los mouse conectados
		for(int i=0; i < _manyMouseMice.Length; i++){
			// Movimiento no-cumulativo
			Vector3 delta = _manyMouseMice[i].Delta;
			float X = delta.x * sens;
			float Y = delta.y * sens;
			// Checa limites (4 lados)
			if (moveUI[i].y < -5  && delta.y < 0) Y = 0;
			if (moveUI[i].y >  5  && delta.y > 0) Y = 0;
			if (moveUI[i].x < -10 && delta.x < 0) X = 0;
			if (moveUI[i].x >  10 && delta.x > 0) X = 0;
			moveUI[i] += new Vector2(X,Y); // Cumulativo
			move[i] = new Vector3(X,Y,actualDistance);
			// Aplica movimientos
			if (move[i].y != 0)	objs[i].transform.Translate(Vector3.down * move[i].y);
			if (move[i].x != 0)	objs[i].transform.Translate(Vector3.right * move[i].x);
			// Reset (No cumulativo)
			move[i].y = 0.0f;
			move[i].x = 0.0f;
		}
	}

	// Mostrar Informacion
	void OnGUI(){
		GUILayout.Label("DEBUG:");
		for(int i=0; i< _manyMouseMice.Length; i++){
			bool[] buttons = _manyMouseMice[i].MouseButtons;
			if(_manyMouseMice[i] != null)
				GUILayout.Label("Mouse[" + i.ToString() + "] : " + moveUI[i] + buttons[0] + buttons[1] + buttons[2] + buttons[3] + buttons[4]);
		}
	}

}