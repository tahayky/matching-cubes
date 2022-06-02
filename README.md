# Matching Cubes Case Project
##  Özellik kullanımları:

### 1.Ambiance(ScriptableObject):
Gökyüzü, arkaplan binaları gibi renklendirilebilir öğelerin renklerini belirler. Asset olarak oluşturulur, pathBuilder'a entegre edilir.<br />
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/ambiance.png?raw=true)<br />
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/ambiances.png?raw=true)<br />
### 2.PathBuilder(MonoBehaviour):
Level pistini ifade eder.<br />
#### a.Ambiance(Ambiance):
Daha önceden oluşturulmuş "Ambiance(asset)" nesnesini level başlangıcında uygular.<br />
#### b.Parts(Part[]):
Koşu pistinin biçimini belirler. Standart pist için "Path", boşluk için "Gap", bitiş için "Finish" ifadeleri kullanılır. Elementlerin sırası Z eksenindeki pozisyonu belirler. Editör modunda düzenlenir.<br />
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/path_builder.png?raw=true)
### 3.Obstacle(MonoBehaviour):
Oyundaki engelleri ifade eder.
#### a.Matrix(Bool[,]):
Girilen 4x4 boolean matrisinin şeklini çıkarır. Editör modunda düzenlenir.<br />
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/obstacle.png?raw=true)
### 4.Cube(MonoBehaviour)
Oyunda toplanan küpleri ifade eder.
#### a.Color(CubeColor):
Küpün rengini belirler. Editör modunda düzenlenir.<br />
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/cube_component.png?raw=true)
![alt text](https://github.com/tahayky/matching-cubes/blob/main/docs/cube.png?raw=true)
