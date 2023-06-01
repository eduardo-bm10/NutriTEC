/// These are the libraries use in the application.
import 'dart:convert';
import 'package:crypto/crypto.dart';
import 'package:flutter/material.dart';
import 'package:validators/validators.dart';
import 'WelcomeScreen.dart';
import 'RegisterScreen.dart';
import 'package:http/http.dart' as http;

// Global variables for the components of the application.
const double font_size = 20;
const double title_size = 32;

/// This is the home page of the application.
class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key});
  @override
  _MyHomePageState createState() => _MyHomePageState();
}


/// This is the stateful class that will be replaced by the home page.
class _MyHomePageState extends State<MyHomePage> {
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
    _getClient("", "");
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
              boxShadow: [
                BoxShadow(color: Colors.blue, spreadRadius: 3),
              ],
            ),
            child: Column(
              children: <Widget>[
                const Expanded(
                  child: Image(image: AssetImage('assets/logoNutriTec.png')),
                ),
                Icon(IconData(0xe043, fontFamily: 'MaterialIcons'),size:60),
                Text(
                  "Correo electrónico: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                       controller: emailAddress ,
                       decoration: InputDecoration( 
                        hintText: "Correo electrónico",
                         errorText: texterror?"Email no válido":null,
                       )
                     ),
                Text(
                  "Contraseña: ",
                  style: TextStyle(fontSize: 25),
                ),
                 TextField(
                  controller: _controllerPassword,
                  obscureText: passwordVisible,
                  decoration: InputDecoration(
                    border: UnderlineInputBorder(),
                    hintText: "Contraseña",
                    suffixIcon: IconButton(
                      icon: Icon(passwordVisible
                          ? Icons.visibility
                          : Icons.visibility_off),
                      onPressed: () {
                        setState(
                          () {
                            passwordVisible = !passwordVisible;
                          },
                        );
                      },
                    ),
                    alignLabelWithHint: false,
                    filled: true,
                  ),
                  keyboardType: TextInputType.visiblePassword,
                  textInputAction: TextInputAction.done,
                ),
                ElevatedButton(
                  style: style,
                  onPressed: () async {
                    if(isEmail(emailAddress.text) && _controllerPassword.text.isNotEmpty){
                            texterror = false;
                            bool estado = _verCliente(emailAddress.text,_controllerPassword.text);
                            _if(estado){
                                _navigateToWelcome(context);
                              }
                        }else{
                            texterror = true;
                    }
                  },
                  child: const Text('Iniciar sesión'),
                ),
                ElevatedButton(
                  style: style,
                  onPressed: () {
                    _navigateToRegister(context);
                  },
                  child: const Text('Registrarse'),
                ),
              ],
            ),
          ),
        ));
  }

  /// This method is used to navigate to the welcome screen.
  void _navigateToWelcome(BuildContext context) {
    Navigator.of(context).push(MaterialPageRoute(builder: (context) => WelcomeScreen()));
  }

  /// This method is used to navigate to the register screen.
  void _navigateToRegister(BuildContext context) {
    Navigator.of(context).push(MaterialPageRoute(builder: (context) => RegisterScreen()));
  }

  /// This method is used to verify if the client exist in the database.
  bool _verCliente(String correo, String password) {
    String generateMd5(String input) {
      return md5.convert(utf8.encode(input)).toString();
    }
    return true;
  }

  Future <bool> _getClient(String correo, String password) async {
    String generateMd5(String input) {
      return md5.convert(utf8.encode(input)).toString();
    }
    String thePassword = generateMd5(password);
    print(await http.get('https://postgresqlapi.azurewebsites.net/api/Patients' as Uri));
    return true;
  }
}