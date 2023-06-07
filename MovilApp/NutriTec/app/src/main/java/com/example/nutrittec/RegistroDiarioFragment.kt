package com.example.nutrittec

import android.annotation.SuppressLint
import android.content.ClipDescription
import android.content.Intent
import android.content.Intent.getIntent
import android.icu.text.SimpleDateFormat
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
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.Call
import okhttp3.Callback
import okhttp3.FormBody
import okhttp3.OkHttpClient
import okhttp3.Request
import okhttp3.RequestBody
import okhttp3.Response
import org.json.JSONArray
import org.json.JSONObject
import java.io.IOException
import java.util.Date
import java.util.Locale


class RegistroDiarioFragment : Fragment() {
    private lateinit var mealTimeSpinner: Spinner
    private lateinit var productName: EditText
    private lateinit var productBarcode: EditText
    private lateinit var productNameButton: Button
    private lateinit var productBarcodeButton: Button
    private lateinit var productFoundTextView: TextView
    private lateinit var registerConsumptionButton: Button
    private lateinit var productFoundTextView2: TextView
    private lateinit var registerConsumptionButton2: Button
    private lateinit var mealtime: String
    private var diccionarioMeals: MutableMap<String, Int> = mutableMapOf()
    private var currentProductCode: Int = 0
    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        return inflater.inflate(R.layout.registro_diario_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        var extras = getActivity()?.getIntent()?.getExtras()

        val cedula = extras?.getString("CedulaPatient")


        mealTimeSpinner = view.findViewById<Spinner>(R.id.mealTimeSpinner)
        productName = view.findViewById(R.id.productNameEditText)
        productBarcode = view.findViewById(R.id.barcodeEditText)
        productNameButton = view.findViewById(R.id.searchByNameButton)
        productBarcodeButton = view.findViewById(R.id.searchByBarcodeButton)
        productFoundTextView = view.findViewById(R.id.productFoundTextView)
        registerConsumptionButton = view.findViewById(R.id.registerConsumptionButton)
        productFoundTextView2 = view.findViewById(R.id.productFoundTextView2)
        registerConsumptionButton2 = view.findViewById(R.id.registerConsumptionButton2)

        callApiMealTimes()
        val dateFormat = SimpleDateFormat("yyyy-MM-dd", Locale.getDefault())
        val currentDate = Date()
        val date = dateFormat.format(currentDate)
        productNameButton.setOnClickListener{
            val description = productName.text.toString()
            callApiProductByDescription(description)
        }

        productBarcodeButton.setOnClickListener {
            val barcode = productBarcode.text.toString().toInt()
            callApiProductByBarcode(barcode)
        }

        registerConsumptionButton.setOnClickListener {
            val description = productName.text.toString()
            if (description!=""){
                callApiProductByDescription(description)
            }

            if(productFoundTextView.text =="Producto/Receta encontrada"){
                var mealTimeId = diccionarioMeals[mealtime]
                val barcodeCode = currentProductCode
                callApiConsumption(cedula.toString(), date,mealTimeId.toString(),barcodeCode.toString())
            }
        }

        registerConsumptionButton2.setOnClickListener {
            val barcode = productBarcode.text.toString().toInt()
            if (barcode>0){
                callApiProductByBarcode(barcode)
            }

            if(productFoundTextView2.text =="Producto/Receta encontrada"){
                var mealTimeId = diccionarioMeals[mealtime]
                callApiConsumption(cedula.toString(), date,mealTimeId.toString(),barcode.toString())
            }
        }

        mealTimeSpinner.onItemSelectedListener = object : AdapterView.OnItemSelectedListener {
            override fun onItemSelected(parent: AdapterView<*>, view: View?, position: Int, id: Long) {
                val selectedItem = parent.getItemAtPosition(position) as String
                mealtime = selectedItem
            }

            override fun onNothingSelected(parent: AdapterView<*>) {
                // Handle the case when nothing is selected
            }
        }
    }

    @SuppressLint("ResourceType")
    private fun parseResponse(responseBody: String?): List<String> {
        val mealTimes = JSONArray(responseBody)
        val names = mutableListOf<String>()
        for (i in 0 until mealTimes.length()) {
            val mealTime = mealTimes.getJSONObject(i)
            val name = mealTime.getString("name")
            diccionarioMeals[name.toString()]=mealTime.getInt("id")
            names.add(name)
        }
        return names
    }

    private fun callApiMealTimes(){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getMealTimes()
            val cuerpo = call.body()
            Log.d("test1",cuerpo.toString())
            requireActivity().runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    val items = parseResponse(responseBody)
                    val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_dropdown_item, items)
                    mealTimeSpinner.adapter = adapter
                    }else {
                            Toast.makeText(requireContext(), "No se obtienen Tiempos de comida", Toast.LENGTH_SHORT).show()
                        }

                }
            }
        }

    private fun callApiProductByBarcode(barcode: Int){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getProductByBarcode(barcode)
            val cuerpo = call.body()
            currentProductCode = (cuerpo?.get("Barcode")?.toString()?.toInt()!!)
            Log.d("test2",cuerpo.toString())
            requireActivity().runOnUiThread {
                if(call.isSuccessful){
                    productFoundTextView2.text = "Producto/Receta encontrada"

                }else {
                    productFoundTextView2.text = "Error al buscar producto/receta."
                }

            }
        }
    }
    private fun callApiProductByDescription(description: String){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getProductByDescription(description)
            val cuerpo = call.body()
            currentProductCode = (cuerpo?.get("Barcode")?.toString()?.toInt()!!)
            Log.d("test3",cuerpo.toString())
            requireActivity().runOnUiThread {
                if(call.isSuccessful){
                    productFoundTextView.text = "Producto/Receta encontrada"

                }else {
                    productFoundTextView.text = "Error al buscar producto/receta."
                }

            }
        }
    }

    private fun callApiConsumption(patientId:String,date:String,mealtimeId:String,barcode:String){
            val apiService = api_service.create()
            Log.d("test4","here")
            CoroutineScope(Dispatchers.IO).launch {
                Log.d("test4","here2")
                val call = apiService.crearConsumo(patientId,date,mealtimeId,barcode)
                val cuerpo = call.body()
                Log.d("test4",call.toString())
                Log.d("test4",cuerpo.toString())
                requireActivity().runOnUiThread {
                    if(call.isSuccessful){
                        Toast.makeText(requireContext(), "Consumo diario registrado", Toast.LENGTH_SHORT).show()
                    }else {
                        Toast.makeText(requireContext(), "No se puede registrar dicho consumo", Toast.LENGTH_SHORT).show()
                    }

                }
            }
        }
}

