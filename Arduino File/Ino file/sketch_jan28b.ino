#define kirmizi 9
#define yesil 10
#define mavi 11
 void setup() 
 {
   Serial.begin(9600);
 }
 void loop() 
 {
  if (Serial.available() == 3)
   {
     analogWrite(kirmizi, Serial.read());
     analogWrite(yesil, Serial.read()); 
     analogWrite(mavi, Serial.read()); 
    }
 }
