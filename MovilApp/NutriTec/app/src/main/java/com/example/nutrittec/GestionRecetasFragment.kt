package com.example.nutrittec

import android.annotation.SuppressLint
import android.content.Intent
import android.os.Bundle
import android.util.Log
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ArrayAdapter
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.Spinner
import android.widget.Toast
import androidx.core.view.allViews
import androidx.fragment.app.Fragment
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import org.json.JSONArray
import org.json.JSONObject

class GestionRecetasFragment : Fragment() {

    private lateinit var productListLayout: LinearLayout
    private lateinit var addProductButton: Button
    private lateinit var addRecipeButton: Button
    private lateinit var seeRecipeButton: Button
    private lateinit var editRecipeButton: Button
    private lateinit var deleteRecipeButton: Button
    private lateinit var deleteProductForRecipe: Button
    private lateinit var productSpinner: Spinner
    private lateinit var recipeName: EditText
    private lateinit var valores: List<String>
    private var diccionarioProducts: MutableMap<String, Int> = mutableMapOf()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.gestion_recetas_fragment, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        // Initialize UI components
        productListLayout = view.findViewById(R.id.productListLayout)
        addProductButton = view.findViewById(R.id.addProductButton)
        addRecipeButton = view.findViewById(R.id.addRecipeButton)
        seeRecipeButton = view.findViewById(R.id.seeRecipesButton)
        editRecipeButton= view.findViewById(R.id.editRecipeButton)
        deleteRecipeButton = view.findViewById(R.id.deleteRecipesButton)
        productSpinner = view.findViewById(R.id.productSpinner)
        deleteProductForRecipe = view.findViewById(R.id.removeProductButton)
        recipeName = view.findViewById(R.id.recipeNameEditText)
        getProducts()

        addProductButton.setOnClickListener {
            // Add a new LinearLayout with product and portion fields
            val sublist = valores.subList(1, valores.size)
            addProductLayout(sublist)
        }

        // Dentro del método addRecipeButton.setOnClickListener()
        addRecipeButton.setOnClickListener {
            val selectedProducts = StringBuilder()
            val portionValues = StringBuilder()

            for (i in 0 until productListLayout.childCount) {
                val childView = productListLayout.getChildAt(i)
                val productSpinner = childView.findViewById<Spinner>(R.id.productSpinner)
                val portionEditText = childView.findViewById<EditText>(R.id.portionEditText)

                val selectedProduct = productSpinner.selectedItem.toString()
                val portionValue = portionEditText.text.toString()


                if (portionValue==null || portionValue=="") {
                    Toast.makeText(requireContext(), "Agregue porciones para cada producto", Toast.LENGTH_SHORT).show()
                    return@setOnClickListener
                }

                if (i > 0) {
                    selectedProducts.append(",")
                    portionValues.append(",")
                }
                var idProduct=diccionarioProducts[selectedProduct].toString()

                selectedProducts.append(idProduct)
                portionValues.append(portionValue)
            }

            if (selectedProducts.isNotEmpty()) {
                Toast.makeText(requireContext(), "Receta creada", Toast.LENGTH_SHORT).show()
                Log.d("Productos", selectedProducts.toString())
                Log.d("Porciones", portionValues.toString())
                if(recipeName.text.isNotEmpty()){
                    callCreateRecipe(recipeName.text.toString(),selectedProducts.toString(),portionValues.toString())
                }
            } else {
                Toast.makeText(requireContext(), "No se puede crear la receta sin productos", Toast.LENGTH_SHORT).show()
            }
        }


        deleteRecipeButton.setOnClickListener {
            val borrado = Intent(requireActivity(), DeleteActivity::class.java)
            requireActivity().startActivity(borrado)
        }

        seeRecipeButton.setOnClickListener {
            val ver = Intent(requireActivity(), seeRecipeActivity::class.java)
            requireActivity().startActivity(ver)
        }

        editRecipeButton.setOnClickListener {
            val ver = Intent(requireActivity(), editRecipeActivity::class.java)
            requireActivity().startActivity(ver)
        }

        deleteProductForRecipe.setOnClickListener {
                // Verificar si hay al menos un productLayout
                if (productListLayout.childCount > 0) {
                    // Obtener el último productLayout
                    val lastProductLayout = productListLayout.getChildAt(productListLayout.childCount - 1)
                    // Eliminar el último productLayout del productListLayout
                    productListLayout.removeView(lastProductLayout)
                } else {
                    Toast.makeText(requireContext(), "No hay productos para eliminar", Toast.LENGTH_SHORT).show()
                }

        }




    }

    // Dentro del método addProductLayout()
    private fun addProductLayout(valores2: List<String>) {
        val inflater = LayoutInflater.from(requireContext())
        val productLayout = inflater.inflate(R.layout.product_layout, productListLayout, false)

        val productSpinner = productLayout.findViewById<Spinner>(R.id.productSpinner)
        val portionEditText = productLayout.findViewById<EditText>(R.id.portionEditText)

        // Validar selección de producto
        val selectedProducts = mutableListOf<String>()
        for (i in 0 until productListLayout.childCount) {
            val childView = productListLayout.getChildAt(i)
            val existingProductSpinner = childView.findViewById<Spinner>(R.id.productSpinner)
            val selectedProduct = existingProductSpinner.selectedItem.toString()
            selectedProducts.add(selectedProduct)
        }

        // Filtrar productos existentes para evitar repeticiones
        val filteredItems = valores2.filter { !selectedProducts.contains(it) }

        if (filteredItems.isNotEmpty()) {
            val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_dropdown_item, filteredItems)
            productSpinner.adapter = adapter

            // Add the new product layout to the productListLayout
            productListLayout.addView(productLayout)
        } else {
            Toast.makeText(requireContext(), "No se pueden repetir productos", Toast.LENGTH_SHORT).show()
        }
    }

    @SuppressLint("ResourceType")
    private fun parseResponse(responseBody: String?): List<String> {
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

    private fun getProducts(){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.getProducts()
            val cuerpo = call.body()
            Log.d("test1",cuerpo.toString())
            requireActivity().runOnUiThread {
                if(call.isSuccessful){
                    val responseBody = cuerpo.toString()
                    val items = parseResponse(responseBody)
                    valores = items
                    val adapter = ArrayAdapter(requireContext(), android.R.layout.simple_spinner_dropdown_item, items)
                    productSpinner.adapter = adapter
                }else {
                    Toast.makeText(requireContext(), "No se obtienen productos", Toast.LENGTH_SHORT).show()
                }

            }
        }
    }

    private fun callCreateRecipe(recipeName:String,selectedProducts: String,portionValues:String){
        val apiService = api_service.create()
        CoroutineScope(Dispatchers.IO).launch {
            val call = apiService.createRecipe(recipeName,selectedProducts,portionValues)
            val cuerpo = call.body()
            Log.d("test1",cuerpo.toString())
            requireActivity().runOnUiThread {
                if(call.isSuccessful){
                    Toast.makeText(requireContext(), "Receta creada", Toast.LENGTH_SHORT).show()
                }else {
                    Toast.makeText(requireContext(), "No se pudo crear la receta", Toast.LENGTH_SHORT).show()
                }
            }
        }
    }
}
