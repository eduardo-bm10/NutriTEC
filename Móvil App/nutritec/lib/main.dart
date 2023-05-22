/// These are the libraries use in the application.
import 'package:flutter/material.dart';

import 'HomeScreen.dart';

// Global variables for the components of the application.
const double font_size = 20;
const double title_size = 32;
/// This is the main method that run the application.
void main() => runApp(
  MyApp()
);

/// This is the class that contains the main widget of the application.
class MyApp extends StatelessWidget {
  const MyApp({super.key});
  static const String _title = 'GymTec';
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: _title,
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      home: MyHomePage(),
    );
  }
}