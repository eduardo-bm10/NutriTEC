package com.example.nutrittec

import android.annotation.SuppressLint
import android.os.Bundle
import android.util.Log
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.Spinner
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.json.JSONArray


class seeRecipeActivity  : AppCompatActivity() {

    private lateinit var recipeSpinner: Spinner
    private lateinit var seeRecipeButton: Button
    private lateinit var descriptionReceta: TextView
    private lateinit var productPortionListReceta: TextView
    private var diccionarioRecipes: MutableMap<String, Int> = mutableMapOf()
    private var productList: String = ""

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.see_recipe_activity)

        // Inicializar vistas
        recipeSpinner = findViewById(R.id.recipeSpinner)
        seeRecipeButton = findViewById(R.id.seeRecipeButton)
        descriptionReceta = findViewById(R.id.descriptionReceta)
        productPortionListReceta = findViewById(R.id.productPortionListReceta)


        callApiGetRecipes()

        // Configurar el botón para mostrar la receta seleccionada
        seeRecipeButton.setOnClickListener {
            val selectedRecipe = recipeSpinner.selectedItem.toString()
            callApiGetRecipes()
            getRecipeData(diccionarioRecipes[selectedRecipe.toString()].toString())

            // Mostrar la descripción y la lista de productos y porciones
            descriptionReceta.text = "Nombre Receta:"+ selectedRecipe
        }
    }

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




    private fun parseResponseData(responseBody: String?,id:String) {
        val allRecipesProductPortion = JSONArray(responseBody)
        for (i in 0 until allRecipesProductPortion.length()) {
            val recetaActual = allRecipesProductPortion.getJSONObject(i)
            productList += "Producto: "+recetaActual.getString("productName")+" con una porción de "+recetaActual.getString("productportion")+"\n"
        }
        Log.d("Productos",productList)
    }


    private fun  callApiGetRecipes(){
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
                    recipeSpinner.adapter = adapter
                }else {
                    Toast.makeText(applicationContext, "No se obtienen recetas", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }

    private fun getRecipeData(id: String){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getRecipesData(id)
            //val call = apiService.getRecipesData(id)
            val cuerpo = call.body()
            runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    parseResponseData(responseBody,id)
                    runOnUiThread {
                        productPortionListReceta.text = "Productos y porciones:\n" + productList
                    }
                }else {
                    Toast.makeText(applicationContext, "No se obtienen recetas", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }

}