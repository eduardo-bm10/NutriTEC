/// These are the libraries use in the application.
import 'dart:convert';
import 'package:crypto/crypto.dart';
import 'package:flutter/material.dart';

// Global variables for the components of the application.
const double font_size = 20;
const double title_size = 32;


/// This is the home page of the application.
class DailyRegisterScreen extends StatefulWidget {
  const DailyRegisterScreen({super.key});
  @override
  _DailyRegisterScreenState createState() => _DailyRegisterScreenState();
}


/// This is the stateful class that will be replaced by the home page.
class _DailyRegisterScreenState extends State<DailyRegisterScreen> {
  /// This is the controller for the text field.
  late final TextEditingController _codeController = TextEditingController();
  late final TextEditingController _nameController = TextEditingController();
  bool texterror = false;
  bool passwordVisible=false;
  String? selectedSucursal = null;
  

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
          "Registro Consumo | NutriTEC",
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
                  "Registro de consumo diario",
                  style: TextStyle(fontSize: title_size, fontWeight: FontWeight.normal),
                ),
                DropdownButtonFormField(
                    items: SucursalesItems,
                      onChanged: (String? newValue) {
                          selectedSucursal = newValue;
                      },
                    value: selectedSucursal,
                    hint: const Text("Seleccione el tiempo de comida"),
                    validator: (value) => value == null ? "Seleccione el tiempo de comida" : null,
                    dropdownColor: Colors.blueAccent,
                    decoration: InputDecoration(
                        enabledBorder: OutlineInputBorder(
                        borderSide: const BorderSide(color: Colors.blue, width: 2),
                        borderRadius: BorderRadius.circular(20),
                        ),
                        border: OutlineInputBorder(
                        borderSide: const BorderSide(color: Colors.blue, width: 2),
                        borderRadius: BorderRadius.circular(20),
                        ),
                        filled: true,
                        fillColor: Colors.blueAccent,
                    ),
                  ),
                const SizedBox(height: 10),
                const Text(
                  "Buscar por nombre: ",
                  style: TextStyle(fontSize: title_size, fontWeight: FontWeight.normal),
                ),
                TextField(
                  controller: _nameController,
                ),
                const Text(
                  "Buscar por código de barras: ",
                  style: TextStyle(fontSize: title_size, fontWeight: FontWeight.normal),
                ),
                TextField(
                  keyboardType: TextInputType.number,
                  controller: _codeController,
                ),
                const SizedBox(height: 10),
                ElevatedButton(
                    style: style,
                    onPressed: () {
                      _registerMeal();
                    },
                    child: const Text('Registrar'),
                ),
              ],
            ),
          ),
        ));
  }

   List<DropdownMenuItem<String>> get SucursalesItems{
    List<DropdownMenuItem<String>> menuItems = <String>["Desayuno","Merienda Mañana","Almuerzo","Merienda Tarde", "Cena"].map((item) {
                          return DropdownMenuItem<String>(
                            value: item,
                            child: Container(
                              height: 50,
                              width: 190,
                              child:Text(item,
                                textAlign: TextAlign.center),
                                
                          ));
                        }).toList();
    return menuItems;
  }

  void _registerMeal(){
    
  }
}