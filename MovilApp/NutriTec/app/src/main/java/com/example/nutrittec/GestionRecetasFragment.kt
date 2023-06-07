package com.example.nutrittec

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.EditText
import android.widget.LinearLayout
import android.widget.Spinner
import androidx.fragment.app.Fragment

class GestionRecetasFragment : Fragment() {

    private lateinit var productListLayout: LinearLayout
    private lateinit var addProductButton: Button

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

        addProductButton.setOnClickListener {
            // Add a new LinearLayout with product and portion fields
            addProductLayout()
        }
    }

    private fun addProductLayout() {
        val inflater = LayoutInflater.from(requireContext())
        val productLayout = inflater.inflate(R.layout.product_layout, productListLayout, false)

        val productSpinner = productLayout.findViewById<Spinner>(R.id.productSpinner)
        val portionEditText = productLayout.findViewById<EditText>(R.id.portionEditText)

        // Add the new product layout to the productListLayout
        productListLayout.addView(productLayout)
    }

    private fun getProducts(){

    }
}
