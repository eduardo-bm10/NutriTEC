package com.example.nutrittec

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import com.google.gson.Gson
import com.google.gson.JsonObject
import com.google.gson.JsonParser
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.GlobalScope
import kotlinx.coroutines.launch
import kotlinx.coroutines.runBlocking
import okhttp3.Call
import okhttp3.Callback
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import okhttp3.ResponseBody
import okhttp3.ResponseBody.Companion.toResponseBody
import org.json.JSONObject
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.IOException
import java.security.MessageDigest

/**
 * Actividad principal de la aplicación que maneja el inicio de sesión y registro de usuarios.
 */
class MainActivity : AppCompatActivity() {

    private lateinit var username: EditText
    private lateinit var password: EditText
    private lateinit var loginButton: Button
    private lateinit var signUpButton: Button

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        username = findViewById(R.id.email)
        password = findViewById(R.id.password)
        loginButton = findViewById(R.id.loginButton)
        signUpButton = findViewById(R.id.signUpButton)

        /**
         * Método ejecutado al hacer clic en el botón de inicio de sesión.
         */
        loginButton.setOnClickListener(View.OnClickListener setOnClickListener@{
            val email = username.text.toString()
            val currentPassword = password.text.toString()

            // Verifica si el correo electrónico tiene un formato válido.
            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Toast.makeText(this@MainActivity, "Correo inválido", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val apiService = api_service.create()
            CoroutineScope(Dispatchers.IO).launch {
                val call = apiService.loginUser(email,currentPassword)
                val cuerpo = call.body()
                runOnUiThread {
                    if(call.isSuccessful){
                        val userType = cuerpo?.get("tipo").toString()
                        Log.d("test",userType)
                        if (userType != null) {
                            if(userType == "\"patient\""){
                                Toast.makeText(this@MainActivity, "Inicio de sesión exitoso", Toast.LENGTH_SHORT).show()
                                val menu = Intent(applicationContext, MenuActivity::class.java)
                                menu.putExtra("CedulaPatient",JSONObject(cuerpo?.get("usuario").toString()).get("id").toString())
                                startActivity(menu)
                                finish()
                            }else {
                                Toast.makeText(this@MainActivity, "Inicio de sesión fallido", Toast.LENGTH_SHORT).show()
                            }
                        }
                    }else{
                        Toast.makeText(this@MainActivity, "Error en la solicitud", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        })

        /**
         * Método ejecutado al hacer clic en el botón de registro.
         */
        signUpButton.setOnClickListener(View.OnClickListener {
            val register = Intent(applicationContext, RegisterActivity::class.java)
            startActivity(register)
            finish()
        })
    }

    /**
     * Calcula el hash MD5 de una cadena de entrada.
     *
     * @param input La cadena de entrada para la cual se calculará el hash MD5.
     * @return El hash MD5 calculado como una cadena hexadecimal.
     */
    fun calculateMD5Hash(input: String): String {
        val md = MessageDigest.getInstance("MD5")
        val messageDigest = md.digest(input.toByteArray())

        // Convierte el arreglo de bytes a una representación hexadecimal
        val hexString = StringBuilder()
        for (byte in messageDigest) {
            val hex = String.format("%02x", byte.toInt() and 0xFF)
            hexString.append(hex)
        }
        return hexString.toString()
    }
}
