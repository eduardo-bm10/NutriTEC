package com.example.nutrittec

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.Spinner
import android.widget.TextView
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
import org.json.JSONArray
import org.json.JSONObject
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.io.IOException
import java.security.MessageDigest
/**
 * Actividad que permite editar una receta.
 */
class editRecipeActivity : AppCompatActivity() {

    private lateinit var productLayoutList: LinearLayout
    private lateinit var editRecipeButton: Button
    private lateinit var setDataButton: Button
    private lateinit var recetasDesplegable: Spinner
    private lateinit var recipeNameText: EditText
    private var diccionarioRecipes: MutableMap<String, Int> = mutableMapOf()
    private var productList: MutableList<String> = mutableListOf()
    private var productPortion: MutableList<String> = mutableListOf()
    private var productPortionUpdated: String = ""
    private var productCode: String = ""
    private var nombreReceta: String = ""
    private lateinit var valores: List<String>
    private var diccionarioProducts: MutableMap<String, Int> = mutableMapOf()
    private var recetaid: Int = 0


    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.edit_recipe_activity)

        // Initialize UI components
        productLayoutList = findViewById(R.id.editProductListLayout)
        editRecipeButton = findViewById(R.id.editRecipeButton)
        setDataButton = findViewById(R.id.setDataButton)
        recetasDesplegable = findViewById(R.id.recetasSpinner)
        recipeNameText = findViewById(R.id.recipeNameEditText)

        // Obtener las recetas
        getRecipes()

        // Set click listener for the editRecipeButton
        editRecipeButton.setOnClickListener {
            productPortionUpdated = ""
            productCode = ""
            for (i in productLayoutList.childCount - 1 downTo 0) {
                if (productLayoutList.childCount > 0) {
                    // Obtener el productLayout en la posición actual
                    val currentProductLayout = productLayoutList.getChildAt(i)
                    val portionEditText = currentProductLayout.findViewById<EditText>(R.id.portionEditText)
                    if (i < productLayoutList.childCount - 1) {
                        productPortionUpdated += ","
                    }
                    productPortionUpdated += portionEditText.text.toString()
                    // Eliminar el productLayout actual del productListLayout
                    productLayoutList.removeView(currentProductLayout)
                }
            }

            // Obtener el código de los productos
            for ((index, product) in productList.withIndex()) {
                productCode += diccionarioProducts[product].toString()
                if (index < productList.size - 1) {
                    productCode += ","
                }
            }

            // Llamar a la API para editar la receta
            callApiEditarReceta(diccionarioRecipes[nombreReceta]?.toInt()!!, recipeNameText.text.toString(),productCode,productPortionUpdated)
        }

        setDataButton.setOnClickListener {
            val selectedRecipe = recetasDesplegable.selectedItem.toString()
            nombreReceta = selectedRecipe
            getRecipes()
            getProducts()
            getRecipeData(diccionarioRecipes[selectedRecipe.toString()].toString())
            // Mostrar la descripción y la lista de productos y porciones
            recipeNameText.setText(selectedRecipe.toString())
        }
    }

    /**
     * Agrega un layout de producto a la lista de productos editables.
     *
     * @param valorProducto El nombre del producto.
     * @param valorPortion La porción del producto.
     */
    private fun addProductLayout(valorProducto: String, valorPortion: String) {
        val inflater = LayoutInflater.from(applicationContext)
        val productLayout = inflater.inflate(R.layout.edit_product_list, productLayoutList, false)

        val portionEditText = productLayout.findViewById<EditText>(R.id.portionEditText)
        val productText = productLayout.findViewById<TextView>(R.id.productTextName)

        portionEditText.setText(valorPortion)
        productText.text = valorProducto
        productLayoutList.addView(productLayout)
    }


    /**
     * Analiza la respuesta de la llamada a la API y devuelve una lista con los nombres de las recetas.
     *
     * @param responseBody La respuesta de la llamada a la API.
     * @return Una lista de nombres de recetas.
     */
    @SuppressLint("ResourceType")
    private fun parseResponse(responseBody: String?): List<String> {
        val allRecipes = JSONArray(responseBody)
        val names = mutableListOf<String>()
        diccionarioRecipes.clear()
        for (i in 0 until allRecipes.length()) {
            val mealTime = allRecipes.getJSONObject(i)
            val name = mealTime.getString("description")
            diccionarioRecipes[name.toString()]=mealTime.getInt("id")
            names.add(name)
        }
        return names
    }

    /**
     * Analiza la respuesta de la llamada a la API y procesa los datos de la receta.
     *
     * @param responseBody La respuesta de la llamada a la API.
     * @param id El ID de la receta.
     */
    private fun parseResponseData(responseBody: String?,id:String) {
        val allRecipesProductPortion = JSONArray(responseBody)
        for (i in 0 until allRecipesProductPortion.length()) {
            val recetaActual = allRecipesProductPortion.getJSONObject(i)
            productList.add(recetaActual.getString("productName"))
            productPortion.add(recetaActual.getString("productportion"))
            addProductLayout(recetaActual.getString("productName"), recetaActual.getString("productportion"))
        }
        Log.d("Productos",productList.toString())
    }

    /**
     * Realiza la llamada a la API para editar una receta.
     *
     * @param id El ID de la receta.
     * @param recipeName El nombre de la receta.
     * @param selectedProducts Los productos seleccionados.
     * @param portionValues Las porciones de los productos.
     */
    private fun callApiEditarReceta(id:Int,recipeName:String,selectedProducts: String,portionValues:String) {
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.updateRecipe(id,recipeName,selectedProducts,portionValues)
            val cuerpo = call.body()
            Log.d("test1",call.toString())
            runOnUiThread {
                if(call.isSuccessful){
                    Toast.makeText(applicationContext, "Receta actualizada", Toast.LENGTH_SHORT).show()
                }else {
                    Toast.makeText(applicationContext, "No se puede actualizar la receta", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    /**
     * Obtiene la lista de recetas desde la API.
     */
    private fun  getRecipes(){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getRecipes()
            val cuerpo = call.body()
            Log.d("test1",cuerpo.toString())
            runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    val items = parseResponse(responseBody)
                    val adapter = ArrayAdapter(applicationContext, android.R.layout.simple_spinner_dropdown_item, items)
                    adapter.notifyDataSetChanged()
                    recetasDesplegable.adapter = adapter
                }else {
                    Toast.makeText(applicationContext, "No se obtienen recetas", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    /**
     * Obtiene los datos de la receta desde la API.
     *
     * @param id El ID de la receta.
     */
    private fun getRecipeData(id: String){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getRecipesData(id)
            val cuerpo = call.body()
            runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    parseResponseData(responseBody,id)
                }else {
                    Toast.makeText(applicationContext, "No se obtienen recetas", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }

    /**
     * Obtiene la lista de productos desde la API.
     */
    private fun getProducts(){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getProducts()
            val cuerpo = call.body()
            Log.d("test1",cuerpo.toString())
            runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    val items = parseResponseProducts(responseBody)
                    valores = items
                }else {
                    Toast.makeText(applicationContext, "No se obtienen productos", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }

    /**
     * Analiza la respuesta de la llamada a la API y devuelve una lista con los nombres de los productos.
     *
     * @param responseBody La respuesta de la llamada a la API.
     * @return Una lista de nombres de productos.
     */
    @SuppressLint("ResourceType")
    private fun parseResponseProducts(responseBody: String?): List<String> {
        val productsList = JSONArray(responseBody)
        val names = mutableListOf<String>()
        for (i in 0 until productsList.length()) {
            val product = productsList.getJSONObject(i)
            val name = product.getString("description")
            diccionarioProducts[name.toString()]=product.getInt("barcode")
            names.add(name)
        }
        return names
    }
}
