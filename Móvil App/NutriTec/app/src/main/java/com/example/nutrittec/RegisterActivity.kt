package com.example.nutrittec


import android.app.DatePickerDialog
import android.content.Intent
import android.icu.util.Calendar
import androidx.appcompat.app.AppCompatActivity
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import okhttp3.Call
import okhttp3.Callback
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import java.io.IOException

class RegisterActivity : AppCompatActivity() {

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

        etFechaNacimiento.setOnClickListener {
            showDatePicker()
        }

        registerButton.setOnClickListener {
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



            if (id.length != 9) {
                Toast.makeText(
                    this@RegisterActivity,
                    "Cedula debe tener 9 digitos",
                    Toast.LENGTH_SHORT
                ).show()
                return@setOnClickListener
            }

            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Toast.makeText(this@RegisterActivity, "Correo invalido", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val url = "https://postgresqlapi.azurewebsites.net/api/Patients?id=$id&firstname=$firstname&lastname1=$lastname1&lastname2=$lastname2&email=$email&password=$password&weight=$weight&bmi=$bmi&address=$address&birthdate=$birthdate&country=$country&maxconsumption=$maxconsumption&waist=$waist&neck=$neck&hips=$hips&musclePercentage=$musclePercentage&fatPercentage=$fatPercentage"

            val client = OkHttpClient()

            val request = Request.Builder()
                .url(url)
                .post(RequestBody.create(null, ByteArray(0))) // No se requiere cuerpo en esta solicitud
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    // Error en la solicitud
                    runOnUiThread {
                        Toast.makeText(this@RegisterActivity, "Error en la solicitud", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onResponse(call: Call, response: Response) {
                    if (response.isSuccessful) {
                        // Registro exitoso
                        runOnUiThread {
                            Toast.makeText(this@RegisterActivity, "Registro exitoso", Toast.LENGTH_SHORT).show()
                            val home = Intent(applicationContext, MainActivity::class.java)
                            startActivity(home)
                            finish()
                        }
                    } else {
                        // Error en la respuesta
                        runOnUiThread {
                            Toast.makeText(this@RegisterActivity, "Error al registrar", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            })
        }
    }

    private fun showDatePicker() {
        val calendar = Calendar.getInstance()
        val year = calendar.get(Calendar.YEAR)
        val month = calendar.get(Calendar.MONTH)
        val day = calendar.get(Calendar.DAY_OF_MONTH)

        val datePickerDialog = DatePickerDialog(this, { _, selectedYear, selectedMonth, selectedDay ->
            // Handle the selected date here
            val formattedDate = "$selectedDay/${selectedMonth + 1}/$selectedYear"
            etFechaNacimiento.setText(formattedDate)
        }, year, month, day)

        datePickerDialog.show()
    }
}
