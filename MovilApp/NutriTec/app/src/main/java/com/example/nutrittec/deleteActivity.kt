package com.example.nutrittec

import android.annotation.SuppressLint
import android.os.Bundle
import android.util.Log
import android.view.View
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.Spinner
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.json.JSONArray

/**
 * Actividad que permite eliminar una receta.
 */
class DeleteActivity : AppCompatActivity() {
    private lateinit var recipeSpinner: Spinner
    private lateinit var deleteRecipeButton: Button
    private var diccionarioRecipes: MutableMap<String, Int> = mutableMapOf()
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.delete_recipe)
        recipeSpinner = findViewById(R.id.recipeSpinner)
        deleteRecipeButton = findViewById(R.id.deleteRecipeButton)

        // Realiza la llamada a la API para obtener las recetas
        callApiGetRecipes()


        // Agrega un listener al botón de eliminación de receta
        deleteRecipeButton.setOnClickListener(View.OnClickListener {
            // Obtiene la receta seleccionada del spinner
            val selectedRecipe = recipeSpinner.getSelectedItem().toString()
            // Obtiene el ID de la receta del diccionario
            val idReceta = diccionarioRecipes[selectedRecipe]!!
            // Elimina la receta por su ID
            elimarRecetaById(idReceta)
            // Realiza la llamada a la API para actualizar la lista de recetas
            callApiGetRecipes()
        })
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
            Log.d("test10",mealTime.toString())
            val name = mealTime.getString("description")
            diccionarioRecipes[name.toString()]=mealTime.getInt("id")
            names.add(name)
        }
        return names
    }

    /**
     * Realiza la llamada a la API para obtener las recetas disponibles.
     */
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

    /**
     * Elimina una receta por su ID.
     *
     * @param id El ID de la receta a eliminar.
     */
    private fun elimarRecetaById(id:Int){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.deleteRecipeById(id)
            runOnUiThread {
                if(call.isSuccessful){
                    Toast.makeText(applicationContext, "Receta eliminada", Toast.LENGTH_SHORT).show()
                }else {
                    Toast.makeText(applicationContext, "No se puede eliminar la receta", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }
}
