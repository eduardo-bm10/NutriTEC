/// These are the libraries use in the application.
import 'dart:convert';
import 'package:crypto/crypto.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:validators/validators.dart';
import 'package:http/http.dart' as http;

import 'WelcomeScreen.dart';


/// Sign Up Screen of the application.
class RegisterScreen  extends StatefulWidget {
  const RegisterScreen ({super.key});
  @override
  _RegisterScreenState createState() => _RegisterScreenState();
}

/// This is the stateful class that will be replaced by the sign up screen.
class _RegisterScreenState extends State<RegisterScreen>{
  /// These are the controllers for the text fields.
  late TextEditingController _cedulaController,_nombreController,_primerApellidoController,_segundoApellidoController,_edadController,_pesoController,_imcController,_direccionController,_correoController,_passwordController;
  TextEditingController dateInput = TextEditingController();
  bool passwordVisible=false;

  @override
  void initState() {
    super.initState();
    /// These are the controllers for the text fields.
    _cedulaController = TextEditingController();
    _nombreController = TextEditingController();
    _primerApellidoController = TextEditingController();
    _segundoApellidoController = TextEditingController();
    _edadController = TextEditingController();
    _pesoController = TextEditingController();
    _imcController = TextEditingController();
    _direccionController = TextEditingController();
    _correoController = TextEditingController();
    _passwordController = TextEditingController();
    passwordVisible = true;
  }

  @override
  Widget build(BuildContext context) {
    /// This is the style of the buttons.
    final ButtonStyle style = ElevatedButton.styleFrom(textStyle: const TextStyle(fontSize: 20));
    /// Scaffold for the sign up screen.
    return Scaffold(
      backgroundColor: Colors.cyan[100],
      appBar: AppBar(title: const Text('Bienvenido a NutriTec')),
      body: Center(
          child: Container(
            child: ListView(
              padding: EdgeInsets.all(20),
              children: <Widget>[
                Expanded(
                  child: Image(image: AssetImage('assets/logoNutriTec.png')),
                ),
                Icon(IconData(0xe043, fontFamily: 'MaterialIcons'),size:60),
                Text(
                  "Cédula: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  keyboardType: TextInputType.number,
                  controller: _cedulaController,
                ),
                Text(
                  "Nombre: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  controller: _nombreController,
                ),
                Text(
                  "Primer Apellido: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  controller: _primerApellidoController,
                ),
                Text(
                  "Segundo Apellido: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  controller: _segundoApellidoController,
                ),
                Text(
                  "Edad: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  keyboardType: TextInputType.number,
                  controller: _edadController,
                ),
                Text(
                  "Fecha de Nacimiento: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
              controller: dateInput,
              //editing controller of this TextField
              decoration: InputDecoration(
                  icon: Icon(Icons.calendar_today), //icon of text field
                  labelText: "Ingrese una fecha" //label text of field
                  ),
              readOnly: true,
              //set it true, so that user will not able to edit text
              onTap: () async {
                DateTime? pickedDate = await showDatePicker(
                    context: context,
                    initialDate: DateTime.now(),
                    firstDate: DateTime(1950),
                    //DateTime.now() - not to allow to choose before today.
                    lastDate: DateTime(2100));
 
                if (pickedDate != null) {
                  String formattedDate =
                      DateFormat('yyyy-MM-dd').format(pickedDate);
                  setState(() {
                    dateInput.text =
                        formattedDate; //set output date to TextField value.
                  });
                } else {}
              }),
                Text(
                  "Peso: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  keyboardType: TextInputType.number,
                  controller: _pesoController,
                ),
                Text(
                  "IMC: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  keyboardType: TextInputType.number,
                  controller: _imcController,
                ),
                Text(
                  "Dirección: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  controller: _direccionController,
                ),
                Text(
                  "Correo Electrónico: ",
                  style: TextStyle(fontSize: 25),
                ),
                TextField(
                  controller: _correoController,
                ),
                Text(
                  "Contraseña: ",
                  style: TextStyle(fontSize: 25),
                ),
                 TextField(
                  controller: _passwordController,
                  obscureText: passwordVisible,
                  decoration: InputDecoration(
                    border: UnderlineInputBorder(),
                    helperStyle:TextStyle(color:Colors.green),
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
                  onPressed: () {
                    if(int.parse(_cedulaController.text) !=0 && _nombreController.text != "" && int.parse(_edadController.text) >0 && isDate(dateInput.text) && double.parse(_pesoController.text) != 0 && double.parse(_imcController.text) > 0 &&_direccionController.text != "" && isEmail(_correoController.text) && _passwordController.text != ""){
                      crearCliente(int.parse(_cedulaController.text), _nombreController.text, _primerApellidoController.text, _segundoApellidoController.text, int.parse(_edadController.text), dateInput.text,double.parse(_pesoController.text), double.parse(_imcController.text), _direccionController.text, _correoController.text, _passwordController.text);
                      _navigateToClass(context);
                    } 
                  },
                  child: const Text('Registrarse'),
                ),
              ],
            ),
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
          ),
        ));
  }


  /// This method is used to navigate to the search class screen.
  void _navigateToClass(BuildContext context) {
    Navigator.of(context).push(MaterialPageRoute(builder: (context) => WelcomeScreen()));
  }

  /// This method is used to create a client.
  void crearCliente(int cedula, String nombre, String apellido1, String apellido2, int edad,String fecha_nacimiento, double peso, double imc,String direccion, String correo, String password){
  DateTime fecha = DateTime.parse(fecha_nacimiento);
  String generateMd5(String input) {
  return md5.convert(utf8.encode(input)).toString();
  }
  }
}