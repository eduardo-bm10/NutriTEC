package com.example.nutrittec

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import okhttp3.Call
import okhttp3.Callback
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import okhttp3.ResponseBody.Companion.toResponseBody
import org.json.JSONObject
import java.io.IOException
import java.security.MessageDigest

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

        loginButton.setOnClickListener(View.OnClickListener setOnClickListener@{
            val email = username.text.toString()
            val currentPassword = password.text.toString()

            val thePassword = calculateMD5Hash(currentPassword)
            if (!android.util.Patterns.EMAIL_ADDRESS.matcher(email).matches()) {
                Toast.makeText(this@MainActivity, "Correo invalido", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val url = "https://postgresqlapi.azurewebsites.net/api/Login/login?email=$email&password=$thePassword"
            val client = OkHttpClient()

            val request = Request.Builder()
                .url(url)
                .post(RequestBody.create(null, ByteArray(0))) // No se requiere cuerpo en esta solicitud
                .build()

            client.newCall(request).enqueue(object : Callback {
                override fun onFailure(call: Call, e: IOException) {
                    // Error en la solicitud
                    runOnUiThread {
                        Toast.makeText(this@MainActivity, "Error en la solicitud", Toast.LENGTH_SHORT).show()
                    }
                }

                override fun onResponse(call: Call, response: Response) {
                    if (response.isSuccessful) {
                        // Login exitoso
                        runOnUiThread{
                            val responseData = response.message.toString()
                            Log.d("test",responseData)


                            val userType = responseData

                            if(!userType.equals("OK")){
                                Toast.makeText(this@MainActivity, "Login fallido", Toast.LENGTH_SHORT).show()
                            }else {
                                Toast.makeText(this@MainActivity, "Login exitoso", Toast.LENGTH_SHORT).show()
                                val menu = Intent(applicationContext, MenuActivity::class.java)
                                menu.putExtra("CedulaPatient","118670690")
                                startActivity(menu)
                                finish()
                            }
                        }
                    } else {
                        // Error en la respuesta
                        runOnUiThread {
                            Toast.makeText(this@MainActivity, "Error al iniciar sesion", Toast.LENGTH_SHORT).show()
                        }
                    }
                }
            })

        })

        signUpButton.setOnClickListener(View.OnClickListener {
            val register = Intent(applicationContext, RegisterActivity::class.java)
            startActivity(register)
            finish()
        })
    }

    fun calculateMD5Hash(input: String): String {
        val md = MessageDigest.getInstance("MD5")
        val messageDigest = md.digest(input.toByteArray())

        // Convert the byte array to hexadecimal representation
        val hexString = StringBuilder()
        for (byte in messageDigest) {
            val hex = String.format("%02x", byte.toInt() and 0xFF)
            hexString.append(hex)
        }
        return hexString.toString()
    }
}
