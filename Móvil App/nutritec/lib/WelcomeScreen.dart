/// These are the libraries use in the application.
import 'dart:convert';
import 'package:crypto/crypto.dart';
import 'package:flutter/material.dart';
import 'package:nutritec/DailyRegisterScreen.dart';

import 'RecipesManagementScreen.dart';

// Global variables for the components of the application.
const double font_size = 20;
const double title_size = 26;


/// This is the home page of the application.
class WelcomeScreen extends StatefulWidget {
  const WelcomeScreen({super.key});
  @override
  _WelcomeScreenState createState() => _WelcomeScreenState();
}


/// This is the stateful class that will be replaced by the home page.
class _WelcomeScreenState extends State<WelcomeScreen> {
  /// This is the controller for the text field.
  late TextEditingController _controllerPassword;
  TextEditingController emailAddress = TextEditingController();
  bool texterror = false;
  bool passwordVisible=false;
  

  @override
  void initState() {
    super.initState();
    _controllerPassword = TextEditingController();
    passwordVisible=true;
    emailAddress.text = "";
  }

  @override
  Widget build(BuildContext context) {
    final ButtonStyle style  = ElevatedButton.styleFrom(textStyle: const TextStyle(fontSize: font_size));
    return Scaffold(
        backgroundColor: Colors.cyan[100],
        appBar: AppBar(
            title: const Text(
          "Bienvenido a NutriTEC",
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
                ElevatedButton(
                    style: style,
                    onPressed: () {
                      _navigateToDailyRegister(context);
                    },
                    child: const Text('Registro Diario'),
                ),
                ElevatedButton(
                    style: style,
                    onPressed: () {
                      _navigateToRecipesManagement(context);
                    },
                    child: const Text('Gestión de Recetas'),
                ),
              ],
            ),
          ),
        ));
  }

  /// This method is used to navigate to the welcome screen.
  void _navigateToDailyRegister(BuildContext context) {
    Navigator.of(context).push(MaterialPageRoute(builder: (context) => DailyRegisterScreen()));
  }

  /// This method is used to navigate to the register screen.
  void _navigateToRecipesManagement(BuildContext context) {
    Navigator.of(context).push(MaterialPageRoute(builder: (context) => RecipesManagementScreen()));
  }

  /// This method is used to verify if the client exist in the database.
  bool _verCliente(String correo, String password) {
    String generateMd5(String input) {
      return md5.convert(utf8.encode(input)).toString();
    }
    return true;
  }
}