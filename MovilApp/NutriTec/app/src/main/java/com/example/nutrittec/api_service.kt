package com.example.nutrittec

import com.google.gson.JsonArray
import com.google.gson.JsonObject
import okhttp3.OkHttpClient
import okhttp3.ResponseBody
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.*
import retrofit2.http.Path

interface api_service {

    @POST("/api/Login/{email}/{password}")
    suspend fun loginUser(
        @Path("email") email: String,
        @Path("password") password: String
    ): Response<JsonObject>



        @POST("/api/Consumptions/post/{patientId}/{date}/{mealtimeId}/{productBarcode}")
        suspend fun crearConsumo(
            @Path("patientId") patientId: String,
            @Path("date") date: String,
            @Path("mealtimeId") mealtimeId: String,
            @Path("productBarcode") productBarcode: String
        ): Response<JsonObject>


    @POST("/api/Patients/post/{id}/{firstname}/{lastname1}/{lastname2}/{email}/{password}/{weight}/{bmi}/{address}/{birthdate}/{country}/{max_consumption}/{waist}/{neck}/{hips}/{musclePercentage}/{fatPercentage}")
    suspend fun createPatient(
        @Path("id") id: String,
        @Path("firstname") firstname: String,
        @Path("lastname1") lastname1: String,
        @Path("lastname2") lastname2: String,
        @Path("email") email: String,
        @Path("password") password: String,
        @Path("weight") weight: Int,
        @Path("bmi") bmi: String,
        @Path("address") address: String,
        @Path("birthdate") birthdate: String,
        @Path("country") country: String,
        @Path("max_consumption") max_consumption: String,
        @Path("waist") waist: String,
        @Path("neck") neck: String,
        @Path("hips") hips: String,
        @Path("musclePercentage") musclePercentage: String,
        @Path("fatPercentage") fatPercentage: String
    ): Response<JsonObject>

    @GET("/api/Mealtime")
    suspend fun getMealTimes(): Response<JsonArray>

    @GET("/api/Product/getByDescription/{description}")
    suspend fun getProductByDescription(
        @Path("description") description: String): Response<JsonObject>

    @GET("/api/Product/getByBarcode/{barcode}")
    suspend fun getProductByBarcode(
        @Path("barcode") barcode: Int
    ): Response<JsonObject>

    @GET("/api/Recipes")
    suspend fun getRecipes(): Response<JsonArray>

    @GET("/api/Product")
    suspend fun getProducts(): Response<JsonArray>

    @GET("/api/RecipeProductAssociations/getProductsAndPortionsFromRecipe/{id}")
    suspend fun getRecipesData(
        @Path("id") id: String
    ): Response<JsonArray>
    @DELETE ("/api/Recipes/delete/{id}")
    suspend fun deleteRecipeById(
        @Path("id") id: Int
    ): Response<JsonObject>

    @PUT ("/api/Recipes/put/{id}/{description}/{barcodeProducts}/{portionProducts}")
    suspend fun updateRecipe(
        @Path("id") id: Int,
        @Path("description") description: String,
        @Path("barcodeProducts") barcodeProducts: String,
        @Path("portionProducts") portionProducts: String
    ): Response<JsonObject>


    @POST ("/api/Functions/createRecipe/{description}/{barcodeProducts}/{portionProducts}")
    suspend fun createRecipe(
        @Path("description") description: String,
        @Path("barcodeProducts") barcodeProducts: String,
        @Path("portionProducts") portionProducts: String
    ): Response<JsonObject>
    @GET("/api/Recipe/get")
    suspend fun getRecipes(
        @Path("barcode") barcode: Int
    ): Response<JsonObject>

    companion object {
        fun create() : api_service {

            val client = OkHttpClient.Builder()
                .build()

            return Retrofit.Builder()
                .client(client)
                .addConverterFactory(GsonConverterFactory.create())
                .baseUrl("https://postgresqlapi.azurewebsites.net")
                .build().create(api_service::class.java)
        }
    }

}