/// These are the libraries use in the application.
import 'dart:convert';
import 'package:crypto/crypto.dart';
import 'package:flutter/material.dart';
import 'package:validators/validators.dart';

// Global variables for the components of the application.
const double font_size = 20;
const double title_size = 32;


/// This is the home page of the application.
class RecipesManagementScreen extends StatefulWidget {
  const RecipesManagementScreen({super.key});
  @override
  _RecipesManagementScreenState createState() => _RecipesManagementScreenState();
}


/// This is the stateful class that will be replaced by the home page.
class _RecipesManagementScreenState extends State<RecipesManagementScreen> {
  /// This is the controller for the text field.
  TextEditingController emailAddress = TextEditingController();
  

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    final ButtonStyle style  = ElevatedButton.styleFrom(textStyle: const TextStyle(fontSize: font_size));
    return Scaffold(
        backgroundColor: Colors.cyan[100],
        appBar: AppBar(
            title: const Text(
          "Recetas | NutriTEC",
          style: TextStyle(fontSize: title_size, fontWeight: FontWeight.normal),
        )),
        body: Center(
          child: Container(
            margin: const EdgeInsets.all(10.0),
            width: 320.0,
            height: 600.0,
            decoration: BoxDecoration(
              borderRadius: BorderRadius.circular(40),
              color: Colors.cyan[100],
              boxShadow: const [
                BoxShadow(color: Colors.blue, spreadRadius: 3),
              ],
            ),
            child: Column(
              children: <Widget>[
                const Expanded(
                  child: Image(image: AssetImage('assets/logoNutriTec.png')),
                ),
                const Text(
                  "Datos de la Receta",
                  style: TextStyle(fontSize: title_size, fontWeight: FontWeight.normal),
                ),
                const Text(
                  "Nombre de la Receta: ",
                ),
                const Text(
                  "Producto: ",
                ),
                const Text(
                  "Porción: ",
                ),
                ElevatedButton(
                    style: style,
                    onPressed: () {
                      _addNewProduct(context);
                    },
                    child: const Text('Añadir Producto'),
                ),
                ElevatedButton(
                    style: style,
                    onPressed: () {
                      _addRecipes(context);
                    },
                    child: const Text('Añadir Recetas'),
                ),
              ],
            ),
          ),
        ));
  }

  /// This method is used to navigate to the welcome screen.
  void _addNewProduct(BuildContext context) {
  }

  /// This method is used to navigate to the register screen.
  void _addRecipes(BuildContext context) {
  }
}