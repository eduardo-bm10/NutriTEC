package com.example.nutrittec

import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.AdapterView
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.fragment.app.Fragment
import okhttp3.Call
import okhttp3.Callback
import okhttp3.FormBody
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import org.json.JSONArray
import java.io.IOException


class RegistroDiarioFragment : Fragment() {
    private lateinit var mealTimeSpinner: Spinner
    private lateinit var productName: EditText
    private lateinit var productBarcode: EditText
    private lateinit var productNameButton: Button
    private lateinit var productBarcodeButton: Button
    private lateinit var productFoundTextView: TextView
    private lateinit var registerConsumptionButton: Button

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.registro_diario_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        mealTimeSpinner = view.findViewById<Spinner>(R.id.mealTimeSpinner)
        productName = view.findViewById(R.id.productNameEditText)
        productBarcode = view.findViewById(R.id.barcodeEditText)
        productNameButton = view.findViewById(R.id.searchByNameButton)
        productBarcodeButton = view.findViewById(R.id.searchByBarcodeButton)
        productFoundTextView = view.findViewById(R.id.productFoundTextView)
        registerConsumptionButton = view.findViewById(R.id.registerConsumptionButton)

        callApiMealTimes()

        productNameButton.setOnClickListener{
            val description = productName.text.toString()
            var url = "https://postgresqlapi.azurewebsites.net/api/Products/description/$description"
            callApiProduct(url)
        }

        productBarcodeButton.setOnClickListener {
            val barcode = productBarcode.text.toString()
            var url = "https://postgresqlapi.azurewebsites.net/api/Products/$barcode"
            callApiProduct(url)
        }

        registerConsumptionButton.setOnClickListener {
            val description = productName.text.toString()
            val barcode = productBarcode.text.toString().toInt()
            if (description!=""){
                var url = "https://postgresqlapi.azurewebsites.net/api/Products/description/$description"
                callApiProduct(url)
            }else if(barcode>=0){
                var url = "https://postgresqlapi.azurewebsites.net/api/Products/$barcode"
                callApiProduct(url)
            }

            if(productFoundTextView.text.toString().equals("Producto/Receta encontrada")){
                val mealTimeId = mealTimeSpinner.selectedItem
                //var url = "https://postgresqlapi.azurewebsites.net/api/Consumptions?patientId=$patientId&date=$date&mealtimeId=$mealtimeId&productBarcode=$barcode"
            }
        }

        mealTimeSpinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>, view: View?, position: Int, id: Long) {
                val selectedItem = parent.getItemAtPosition(position) as String
                // Handle the selected item
            }

            override fun onNothingSelected(parent: AdapterView<*>) {
                // Handle the case when nothing is selected
            }
        }
    }

    private fun parseResponse(responseBody: String?): List<String> {
        val mealTimes = JSONArray(responseBody)
        val names = mutableListOf<String>()

        for (i in 0 until mealTimes.length()) {
            val mealTime = mealTimes.getJSONObject(i)
            val name = mealTime.getString("name")
            names.add(name)
        }
        return names
    }

    private fun callApiMealTimes(){
        val url = "https://postgresqlapi.azurewebsites.net/api/MealTimes"

        val client = OkHttpClient()


        val request = Request.Builder()
            .url(url)
            .get()
            .build()

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                requireActivity().runOnUiThread {
                    Toast.makeText(requireContext(), "Error en la solicitud", Toast.LENGTH_SHORT).show()
                }
            }

            override fun onResponse(call: Call, response: Response) {
                if (response.isSuccessful) {
                    val responseBody = response.body?.string()
                    val items = parseResponse(responseBody)
                    requireActivity().runOnUiThread {
                        val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_dropdown_item, items)
                        mealTimeSpinner.adapter = adapter
                    }
                } else {
                    requireActivity().runOnUiThread {
                        Toast.makeText(requireContext(), "Error en la solicitud", Toast.LENGTH_SHORT).show()
                    }
                }
            }
        })
    }

    private fun callApiProduct(url: String){
        val client = OkHttpClient()

        val request = Request.Builder()
            .url(url)
            .get()
            .build()

        client.newCall(request).enqueue(object : Callback {
            override fun onFailure(call: Call, e: IOException) {
                // Error en la solicitud
                requireActivity().runOnUiThread {
                    productFoundTextView.text = "Error al buscar producto/receta."
                }
            }

            override fun onResponse(call: Call, response: Response) {
                if (response.isSuccessful) {
                    requireActivity().runOnUiThread {
                        productFoundTextView.text = "Producto/Receta encontrada"
                    }
                } else {
                    // Error en la respuesta
                    requireActivity().runOnUiThread {
                        productFoundTextView.text = "Producto/Receta no encontrado"
                    }
                }
            }
        })
    }

    //private fun callApiConsumption(){

    //}
}

