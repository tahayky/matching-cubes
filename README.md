# Matching Cubes Case Project
##Özellik kullanımları:

###1.Ambiance(ScriptableObject):
Gökyüzü, arkaplan binaları gibi renklendirilebilir öğelerin renklerini belirler. Asset olarak oluşturulur, pathBuilder'a entegre edilir.

###2.PathBuilder(MonoBehaviour):
Level pistini ifade eder.
####a.Ambiance(Ambiance):
Daha önceden oluşturulmuş "Ambiance(asset)" nesnesini level başlangıcında uygular.
####b.Parts(Part[]):
Koşu pistinin biçimini belirler. Standart pist için "Path", boşluk için "Gap", bitiş için "Finish" ifadeleri kullanılır. Elementlerin sırası Z eksenindeki pozisyonu belirler. Editör modunda düzenlenir.

###3.Obstacle(MonoBehaviour):
Oyundaki engelleri ifade eder.
####a.Matrix(Bool[,]):
Girilen 4x4 boolean matrisinin şeklini çıkarır. Editör modunda düzenlenir.

###4.Cube(MonoBehaviour)
Oyunda toplanan küpleri ifade eder.
####a.Color(CubeColor):
Küpün rengini belirler. Editör modunda düzenlenir.
