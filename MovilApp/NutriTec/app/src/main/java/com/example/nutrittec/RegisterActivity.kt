package com.example.nutrittec


import android.app.DatePickerDialog
import android.app.PendingIntent.getActivity
import android.content.Intent
import android.icu.util.Calendar
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.util.Log
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.Call
import okhttp3.Callback
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import retrofit2.http.Path
import java.io.IOException

class RegisterActivity : AppCompatActivity() {

    // Declaración de variables de los elementos de la interfaz de usuario
    private lateinit var etCedula: EditText
    private lateinit var etNombre: EditText
    private lateinit var etPrimerApellido: EditText
    private lateinit var etSegundoApellido: EditText
    private lateinit var etFechaNacimiento: EditText
    private lateinit var etPeso: EditText
    private lateinit var etIMC: EditText
    private lateinit var etCorreo: EditText
    private lateinit var etContraseña: EditText
    private lateinit var etAddress: EditText
    private lateinit var etCountry: EditText
    private lateinit var etWaist: EditText
    private lateinit var etNeck: EditText
    private lateinit var etHips: EditText
    private lateinit var etMusclePercentage: EditText
    private lateinit var etFatPercentage: EditText
    private lateinit var registerButton: Button
    private lateinit var etMaxConsumption: EditText


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)

        // Inicialización de los elementos de la interfaz de usuario
        etCedula = findViewById(R.id.etCedula)
        etNombre = findViewById(R.id.etNombre)
        etPrimerApellido = findViewById(R.id.etPrimerApellido)
        etSegundoApellido = findViewById(R.id.etSegundoApellido)
        etFechaNacimiento = findViewById(R.id.etFechaNacimiento)
        etPeso = findViewById(R.id.etPeso)
        etIMC = findViewById(R.id.etIMC)
        etCorreo = findViewById(R.id.etCorreo)
        etContraseña = findViewById(R.id.etContraseña)
        etAddress = findViewById(R.id.etAddress)
        etCountry = findViewById(R.id.etCountry)
        etWaist = findViewById(R.id.etWaist)
        etNeck = findViewById(R.id.etNeck)
        etHips = findViewById(R.id.etHips)
        etMusclePercentage = findViewById(R.id.etMusclePercentage)
        etFatPercentage = findViewById(R.id.etFatPercentage)
        registerButton = findViewById(R.id.registerButton)
        etMaxConsumption = findViewById(R.id.etMaxCom)

        // Asignar un listener al EditText de fecha de nacimiento para mostrar el DatePicker
        etFechaNacimiento.setOnClickListener {
            showDatePicker()
        }

        // Asignar un listener al botón de registro
        registerButton.setOnClickListener {
            // Obtener los valores ingresados por el usuario
            val id = etCedula.text.toString()
            val firstname = etNombre.text.toString()
            val lastname1 = etPrimerApellido.text.toString()
            val lastname2 = etSegundoApellido.text.toString()
            val email = etCorreo.text.toString()
            val password = etContraseña.text.toString()
            val weight = etPeso.text.toString()
            val bmi = etIMC.text.toString()
            val address = etAddress.text.toString()
            val birthdate = etFechaNacimiento.text.toString()
            val country = etCountry.text.toString()
            val maxconsumption = etMaxConsumption.text.toString()
            val waist = etWaist.text.toString()
            val neck = etNeck.text.toString()
            val hips = etHips.text.toString()
            val musclePercentage = etMusclePercentage.text.toString()
            val fatPercentage = etFatPercentage.text.toString()



            // Validar los datos ingresados por el usuario

            // Validar la longitud de la cédula
            if (id.length != 9) {
                Toast.makeText(
                    this@RegisterActivity,
                    "Cedula debe tener 9 digitos",
                    Toast.LENGTH_SHORT
                ).show()
                return@setOnClickListener
            }

            // Validar el formato del correo electrónico
            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Toast.makeText(this@RegisterActivity, "Correo invalido", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            // Llamar a la función de creación de paciente
            createPatient(id,firstname, lastname1, lastname2, email, password, weight, bmi, address, birthdate, country, maxconsumption, waist, neck, hips, musclePercentage, fatPercentage)
        }
    }

    /**
     * Función para mostrar el DatePicker al hacer clic en el EditText de fecha de nacimiento
     */
    private fun showDatePicker() {
        // Obtener la fecha actual
        val calendar = Calendar.getInstance()
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        // Crear el DatePickerDialog y mostrarlo
        val datePickerDialog = DatePickerDialog(this, { _, selectedYear, selectedMonth, selectedDay ->
            // Handle the selected date here
            val formattedDate = "$selectedDay/${selectedMonth + 1}/$selectedYear"
            etFechaNacimiento.setText(formattedDate)
        }, year, month, day)


        datePickerDialog.show()
    }

    /**
     * Función para crear un nuevo paciente
      */
    private fun createPatient(id: String, firstname: String, lastname1: String, lastname2: String,
                              email: String, password: String, weight: String, bmi: String,
                              address: String, birthdate: String, country: String, maxconsumption: String,
                              waist: String, neck: String, hips: String, musclePercentage: String,
                              fatPercentage: String){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            // Llamar al método de creación de paciente en el servicio de la API
            val call = apiService.createPatient(id,firstname,lastname1,
                                                lastname2,email,password,weight.toInt(),bmi,
                                                address,birthdate,country,maxconsumption,waist,
                                                neck,hips,musclePercentage, fatPercentage)
            Log.d("testeo",call.toString())
            // Actualizar la interfaz de usuario en el hilo principal
            runOnUiThread {
                if(call.isSuccessful){
                    Toast.makeText(this@RegisterActivity, "Registro exitoso", Toast.LENGTH_SHORT).show()
                    val home = Intent(applicationContext, MainActivity::class.java)
                    startActivity(home)
                    finish()
                }else {
                    Toast.makeText(this@RegisterActivity, "Error al registrar", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }
}
