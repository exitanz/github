
import ddf.minim.*;
Minim minim;
AudioPlayer sf[];
String btnLabel[];
int btnNum;

float a; 
float z;
float easing = 0.1;

int[] x, y, lr;
int[] r, g, b;
int[] c;
int count = 0;
int count2;
int timeLimit = 10;
int timeLimit2 = 30;
int countDown, countDown2;
PImage img, img2, img3;
PFont font;

final int MAX = 40;
final int HIT = -1;
final int NOHIT = 0;
void setup()
{
    int i;
    x = new int[MAX];
    y = new int[MAX];
    lr = new int[MAX];
    r = new int[MAX];
    g = new int[MAX];
    b = new int[MAX];
    c = new int[MAX];
    textSize(30);
    font = createFont("メイリオ", 30);
    textFont(font);
    size(1280,720);
    for (i = 0; i< MAX; ++i) 
    {
      x[i] = int(random(1000));
      y[i] = int(random(600));
      r[i] = int(random(256));
      g[i] = int(random(256));
      b[i] = int(random(256));
      lr[i] = int(random(2))*2-1;
      c[i] = 0;
    }
    img = loadImage("./data/taiyaki.png");
    img2 = loadImage("./data/ninja.png");
    img.resize( img.width/5, img.height/5 );
    img2.resize( img2.width/3, img2.height/3 );
    
    minim = new Minim(this);
    btnNum = 3;
    sf = new AudioPlayer[btnNum];
    sf[0] = minim.loadFile("./data/n82.mp3");
    sf[1] = minim.loadFile("./data/powerup02.mp3");
    sf[2] = minim.loadFile("./data/jingle.mp3");
}

void draw() 
{
    draw1();
    draw2();
    sf[0].play();
    
    float dx = mouseX - a;
    if(abs(dx) > 1) 
    {
      a += dx * easing;
    }
    float dy = mouseY - z;
    if(abs(dy) > 1) 
    {
      z += dy * easing;
    }
    image(img2, a, z);

    int ms = millis()/1000;
    println(ms);
    fill(0);
    countDown = timeLimit - ms;
    if(countDown > 0)
    {
      text("たい焼きを多く取ろう！", 100, 300);//taiyaki wo torou
      text("用意..."+countDown, 100, 400);//youi...
    } else if(countDown == 0)
    {
      text("開始！", 100, 400);//kaisi!
    }
    else
    {
      for (int i = 0; i < MAX; ++i) 
      {
        int tx = x[i];
        int ty = y[i];
        
        text("得点:", 800, 100);//tokuten
        float distance = sqrt(sq(mouseX -tx) + sq(mouseY -ty));
        if (c[i] == NOHIT && (20 + 65)/2 > distance) 
        {
            c[i] = HIT;
            count++;
            sf[1].play(2);
        }
        if (c[i] == HIT && (20 + 65)/2 < distance) 
        {
            c[i] = NOHIT;
        }
        if(countDown2 <= 0)
        {
            break;
        }
      }
      
      int ms2 = millis()/1000;
      println(ms2);
      fill(0);
      countDown2 = timeLimit2 - ms2;
      if(countDown2 > 0)
      {
        if(countDown2 <= 10)
        {
          img2 = loadImage("./data/ninja2.png");
          img2.resize( img2.width/5, img2.height/5 );
        }
        text("残り時間: "+countDown2, 100, 100);//nokorijikan
      }
      else if(countDown2 <= 0)
      {
        text("時間切れ!", 100, 100);//jikanngire!
        textSize(100);
        text("終了！天晴れ！", 100, 400);//syuuryou appare!
        draw3();
      }
   }
}


void draw1()
{
  int i;
  background(223);
  for (i = 0; i < MAX; ++i) 
  {
      x[i] += lr[i];
      y[i] += lr[i];
      if (x[i] > width) 
      {
        lr[i] = -3;
      }
      else if (x[i] < 0) 
      {
        lr[i] = 4;
      }
      else if (y[i] < 0) 
      {
        lr[i] = 3;
       }
     else if (y[i] > height) 
      {
        lr[i] = -2;
      }
    image(img,x[i], y[i]);
  }
}

void draw2() 
{
  textSize(100);
  text(count, 1050, 100);//count
}

void draw3()
{
  sf[0].close();
  sf[1].close();
  sf[2].play();
}
