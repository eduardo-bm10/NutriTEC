package com.example.nutrittec

import com.google.gson.JsonArray
import com.google.gson.JsonObject
import okhttp3.OkHttpClient
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import retrofit2.http.*
import retrofit2.http.Path

/**
 * Interfaz que define los servicios de la API.
 */
interface api_service {

    /**
     * Realiza la autenticación de un usuario.
     *
     * @param email El email del usuario.
     * @param password La contraseña del usuario.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @POST("/api/Login/{email}/{password}")
    suspend fun loginUser(
        @Path("email") email: String,
        @Path("password") password: String
    ): Response<JsonObject>


    /**
     * Crea un consumo.
     *
     * @param patientId El ID del paciente.
     * @param date La fecha del consumo.
     * @param mealtimeId El ID de la comida.
     * @param productBarcode El código de barras del producto.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @POST("/api/Consumptions/post/{patientId}/{date}/{mealtimeId}/{productBarcode}")
    suspend fun crearConsumo(
        @Path("patientId") patientId: String,
        @Path("date") date: String,
        @Path("mealtimeId") mealtimeId: String,
        @Path("productBarcode") productBarcode: String
    ): Response<JsonObject>

    /**
     * Crea un paciente.
     *
     * @param id El ID del paciente.
     * @param firstname El nombre del paciente.
     * @param lastname1 El primer apellido del paciente.
     * @param lastname2 El segundo apellido del paciente.
     * @param email El email del paciente.
     * @param password La contraseña del paciente.
     * @param weight El peso del paciente.
     * @param bmi El índice de masa corporal (BMI) del paciente.
     * @param address La dirección del paciente.
     * @param birthdate La fecha de nacimiento del paciente.
     * @param country El país del paciente.
     * @param max_consumption El consumo máximo del paciente.
     * @param waist La medida de la cintura del paciente.
     * @param neck La medida del cuello del paciente.
     * @param hips La medida de las caderas del paciente.
     * @param musclePercentage El porcentaje de masa muscular del paciente.
     * @param fatPercentage El porcentaje de grasa corporal del paciente.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
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

    /**
     * Obtiene los horarios de las comidas.
     *
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonArray].
     */
    @GET("/api/Mealtime")
    suspend fun getMealTimes(): Response<JsonArray>

    /**
     * Obtiene un producto por su descripción.
     *
     * @param description La descripción del producto.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @GET("/api/Product/getByDescription/{description}")
    suspend fun getProductByDescription(
        @Path("description") description: String): Response<JsonObject>

    /**
     * Obtiene un producto por su código de barras.
     *
     * @param barcode El código de barras del producto.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @GET("/api/Product/getByBarcode/{barcode}")
    suspend fun getProductByBarcode(
        @Path("barcode") barcode: Int
    ): Response<JsonObject>

    /**
     * Obtiene todas las recetas.
     *
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonArray].
     */
    @GET("/api/Recipes")
    suspend fun getRecipes(): Response<JsonArray>

    /**
     * Obtiene todas las recetas.
     *
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonArray].
     */
    @GET("api/Product/getByStatus/true")
    suspend fun getProducts(): Response<JsonArray>

    /**
     * Obtiene los datos de las recetas.
     *
     * @param id El ID de la receta.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonArray].
     */
    @GET("/api/RecipeProductAssociations/getProductsAndPortionsFromRecipe/{id}")
    suspend fun getRecipesData(
        @Path("id") id: String
    ): Response<JsonArray>

    /**
     * Elimina una receta por su ID.
     *
     * @param id El ID de la receta a eliminar.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @DELETE ("/api/Recipes/delete/{id}")
    suspend fun deleteRecipeById(
        @Path("id") id: Int
    ): Response<JsonObject>

    /**
     * Actualiza una receta.
     *
     * @param id El ID de la receta a actualizar.
     * @param description La descripción de la receta actualizada.
     * @param barcodeProducts Los códigos de barras de los productos de la receta actualizada.
     * @param portionProducts Las porciones de los productos de la receta actualizada.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @PUT ("/api/Recipes/put/{id}/{description}/{barcodeProducts}/{portionProducts}")
    suspend fun updateRecipe(
        @Path("id") id: Int,
        @Path("description") description: String,
        @Path("barcodeProducts") barcodeProducts: String,
        @Path("portionProducts") portionProducts: String
    ): Response<JsonObject>


    /**
     * Crea una receta.
     *
     * @param description La descripción de la receta.
     * @param barcodeProducts Los códigos de barras de los productos de la receta.
     * @param portionProducts Las porciones de los productos de la receta.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @POST ("/api/Functions/createRecipe/{description}/{barcodeProducts}/{portionProducts}")
    suspend fun createRecipe(
        @Path("description") description: String,
        @Path("barcodeProducts") barcodeProducts: String,
        @Path("portionProducts") portionProducts: String
    ): Response<JsonObject>

    /**
     * Obtiene las recetas por código de barras.
     *
     * @param barcode El código de barras de la receta.
     * @return [Response] que contiene el resultado de la solicitud. El cuerpo de la respuesta es un [JsonObject].
     */
    @GET("/api/Recipe/get")
    suspend fun getRecipes(
        @Path("barcode") barcode: Int
    ): Response<JsonObject>

    companion object {
        /**
         * Crea una instancia de [api_service].
         *
         * @return La instancia de [api_service].
         */
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