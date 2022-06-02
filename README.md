# Matching Cubes Case Project
##  Özellik kullanımları:

### 1.Ambiance(ScriptableObject):
/r Gökyüzü, arkaplan binaları gibi renklendirilebilir öğelerin renklerini belirler. Asset olarak oluşturulur, pathBuilder'a entegre edilir.
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/ambiance.png?raw=true)
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/ambiances.png?raw=true)
### 2.PathBuilder(MonoBehaviour):
/r Level pistini ifade eder.
#### a.Ambiance(Ambiance):
/r Daha önceden oluşturulmuş "Ambiance(asset)" nesnesini level başlangıcında uygular.
#### b.Parts(Part[]):
/r Koşu pistinin biçimini belirler. Standart pist için "Path", boşluk için "Gap", bitiş için "Finish" ifadeleri kullanılır. Elementlerin sırası Z eksenindeki pozisyonu /r belirler. Editör modunda düzenlenir.
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/path_builder.png?raw=true)
### 3.Obstacle(MonoBehaviour):
/r Oyundaki engelleri ifade eder.
#### a.Matrix(Bool[,]):
/r Girilen 4x4 boolean matrisinin şeklini çıkarır. Editör modunda düzenlenir.
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/obstacle.png?raw=true)
### 4.Cube(MonoBehaviour)
/r Oyunda toplanan küpleri ifade eder.
#### a.Color(CubeColor):
/r Küpün rengini belirler. Editör modunda düzenlenir.
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/cube_component.png?raw=true)
/r ![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/cube.png?raw=true)
