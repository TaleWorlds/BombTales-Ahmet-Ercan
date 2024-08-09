using System.Text;

public class Bomb
{

    internal static Bomb _singleton = null!;

    private enum WireColor
    {
        Kırmızı,
        Mavi,
        Beyaz,
        Yeşil,
        Sarı,
        Siyah
    }
    private enum CompWireType
    {
        C,
        S,
        D,
        P,
        B
    }
    private class BombMemory
	{
		private struct Button
		{
			private int _position;
			private int _label;
			public int Position 
			{ 
				get => _position;
				private set => _position = value;
			}
			public int Label 
			{ 
				get => _label; 
				private set => _label = value; 
			}
			public Button( int position, int label )
			{
				_position = position;
				_label = label;
			}
		}

        private void OnBombDetonated()
        {
            _bombDetonated = true;
        }
        private void OnMemoryDeactivated()
        {
            _memoryDeactivated = true;
        }

        public BombMemory(Bomb bomb)
        {
            _bomb = bomb;
            bomb.BombDetonated += OnBombDetonated;
            bomb.MemoryDeactivated += OnMemoryDeactivated;

            var rd = new Random();

            _displayedNum = rd.Next(1, 4);

            _labels = _labels.OrderBy(x => rd.Next()).ToList();

            _buttons[0] = new Button(1, _labels[0]);
            _buttons[1] = new Button(2, _labels[1]);
            _buttons[2] = new Button(3, _labels[2]);
            _buttons[3] = new Button(4, _labels[3]);

        }

        private List<int> _labels = new List<int> { 1, 2, 3, 4 };

        private Bomb _bomb;
        private bool _bombDetonated = false;
        private bool _memoryDeactivated = false;
        private int _memoryState = 1;
		private int _displayedNum;

		private Button _firstStagePressedButton;
		private Button _secondStagePressedButton;
		private Button _thirdStagePressedButton;
        private Button _fourthStagePressedButton;

		private Button[] _buttons = new Button[4];

        private int _currentStage = 1;

        public int CurrentStage
		{
			get => _currentStage;
			private set => _currentStage = value;
		}

        private void _resetStage()
        {
            Console.WriteLine("*Yanlış butona basıldı. Stage 1'e geri dönüldü.*");
            _currentStage = 1;
        }
        private void _nextStage()
        {
            if (!_bombDetonated)
            {
                if (_currentStage != 5)
                {
                    Console.WriteLine("*Stage " + _currentStage + " tamamlandı. Yeni stage'e geçildi.*");
                    Random random = new Random();

                    _displayedNum = random.Next(1, 4);

                    _labels = _labels.OrderBy(x => random.Next()).ToList();

                    _buttons[0] = new Button(1, _labels[0]);
                    _buttons[1] = new Button(2, _labels[1]);
                    _buttons[2] = new Button(3, _labels[2]);
                    _buttons[3] = new Button(4, _labels[3]);

                    _currentStage++;
                    DisplayScreen();
                }
                else
                {
                    _bomb._isMemoryActive = false;
                    Console.WriteLine("*Memory deaktive edildi.*");
                    _bomb._checkBombState();
                }
            }
            else
            {
                Console.WriteLine("Bomba çoktan patladı.");
            }
        }
        private void _memoryFail()
        {
            Console.WriteLine("*Memory Hatası!*");
            _memoryState = 0;
        }
        public void DisplayScreen()
		{
            if (!_memoryDeactivated)
            {
                if (!_bombDetonated)
                {
                    Console.WriteLine("_________________________________________________");
                    Console.WriteLine("Aktif stage: " + _currentStage);
                    Console.WriteLine("Memory ekranı " + _displayedNum + " sayısını gösteriyor.");
                    Console.WriteLine("Buton 0: Üzerinde " + _buttons[0].Label + " yazıyor.");
                    Console.WriteLine("Buton 1: Üzerinde " + _buttons[1].Label + " yazıyor.");
                    Console.WriteLine("Buton 2: Üzerinde " + _buttons[2].Label + " yazıyor.");
                    Console.WriteLine("Buton 3: Üzerinde " + _buttons[3].Label + " yazıyor.");
                    Console.WriteLine("_________________________________________________");
                }
                else
                {
                    Console.WriteLine("Bomba çoktan patladı.");
                }
            }
            else
            {
                Console.WriteLine("Memory çoktan deaktive edildi.");
            }
        }
        public void PressButton(int buttonNum)
        {
            if (!_memoryDeactivated)
            {
                if (!_bombDetonated)
                {
                    Console.WriteLine(buttonNum + " indeksli tuşa bastın.");
                    if (buttonNum < 0 || buttonNum > 3 || _memoryState == 0)
                    {
                        Console.WriteLine("Geçersiz buton.");
                        return;
                    }
                    switch (_currentStage)
                    {
                        case 1:
                            switch (_displayedNum)
                            {
                                case 1:
                                case 2:
                                    if (_buttons[buttonNum].Position == 2)
                                    {
                                        _firstStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 3:
                                    if (_buttons[buttonNum].Position == 3)
                                    {
                                        _firstStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 4:
                                    if (_buttons[buttonNum].Position == 4)
                                    {
                                        _firstStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                            }
                            break;

                        case 2:
                            switch (_displayedNum)
                            {
                                case 1:
                                    if (_buttons[buttonNum].Label == 4)
                                    {
                                        _secondStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 2:
                                    if (_buttons[buttonNum].Position == _firstStagePressedButton.Position)
                                    {
                                        _secondStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 3:
                                    if (_buttons[buttonNum].Position == 1)
                                    {
                                        _secondStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 4:
                                    if (_buttons[buttonNum].Position == _firstStagePressedButton.Position)
                                    {
                                        _secondStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                            }
                            break;
                        case 3:
                            switch (_displayedNum)
                            {
                                case 1:
                                    if (_buttons[buttonNum].Label == _secondStagePressedButton.Label)
                                    {
                                        _thirdStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 2:
                                    if (_buttons[buttonNum].Label == _firstStagePressedButton.Label)
                                    {
                                        _thirdStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 3:
                                    if (_buttons[buttonNum].Position == 3)
                                    {
                                        _thirdStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 4:
                                    if (_buttons[buttonNum].Label == 4)
                                    {
                                        _thirdStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                            }
                            break;
                        case 4:
                            switch (_displayedNum)
                            {
                                case 1:
                                    if (_buttons[buttonNum].Position == _firstStagePressedButton.Position)
                                    {
                                        _fourthStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 2:
                                    if (_buttons[buttonNum].Position == 1)
                                    {
                                        _fourthStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 3:
                                    if (_buttons[buttonNum].Position == _secondStagePressedButton.Position)
                                    {
                                        _fourthStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 4:
                                    if (_buttons[buttonNum].Position == _secondStagePressedButton.Position)
                                    {
                                        _fourthStagePressedButton = _buttons[buttonNum];
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                            }
                            break;
                        case 5:
                            switch (_displayedNum)
                            {
                                case 1:
                                    if (_buttons[buttonNum].Label == _firstStagePressedButton.Label)
                                    {
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 2:
                                    if (_buttons[buttonNum].Label == _secondStagePressedButton.Label)
                                    {
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 3:
                                    if (_buttons[buttonNum].Label == _fourthStagePressedButton.Label)
                                    {
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                                case 4:
                                    if (_buttons[buttonNum].Position == _thirdStagePressedButton.Label)
                                    {
                                        _nextStage();
                                    }
                                    else
                                    {
                                        _resetStage();
                                    }
                                    break;
                            }
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Bomba çoktan patladı.");
                }
            }
            else
            {
                Console.WriteLine("Memory çoktan deaktive edildi.");
            }
        }
    }

    private struct _wire
	{
		private WireColor _color;
		private int		_state;

		public _wire(WireColor color, int state)
		{ 
			this._color = color; 
			this._state = state;

		}

		public WireColor Color
		{
			get => _color;
		}
		public int State
		{ 
			get => _state;
			private set => _state = value; 
		}

	}
	private struct _complexWire
	{
		private CompWireType _type;
		private int		_state;

		public _complexWire(CompWireType type)
		{
			this._type = type;
			this._state = 1;
		}
        public _complexWire(CompWireType type, int state)
        {
            this._type = type;
            this._state = state;
        }
        public CompWireType Type
		{
			get => _type;
			private set =>_type = value;
		}
		public int State
		{ 
			get => _state; 
			private set => _state = value; 
		}
	}
	private struct _button
	{
		private string _text;
		private string _color;

		private _button(string color, string text)
		{
			this._color = color;
			this._text = text;
		}
		public string Text
		{
			get => _text;
			private set => _text = value;
		}
	}

    private string[] _availableFunctions = new[]
    {
        "DisplayMemoryScreen",
        "PressMemoryButton",
        "CheckBombModules",
        "CheckBombAttributes",
        "CheckWires",
        "CutWire",
        "CutComplexWire",
        "GetWireManuel",
        "GetComplexWireManuel",
        "GetMemoryStageManuel",
        "GetAvailableFunctions"
    };

    internal event Action BombDetonated;
    internal event Action _gameEndedEvent;

    protected virtual void OnBombDetonated()
    {
        Console.WriteLine("Bomba Patladı!");
        Console.WriteLine("Oyunu Kaybettiniz!");
        BombDetonated?.Invoke();
    }
    private event Action MemoryDeactivated;
    protected virtual void OnMemoryDeactivated()
    {
        MemoryDeactivated?.Invoke();
    }

    private static readonly string[] _portTypes =
    {
        "DVI-D",
        "Parallel",
        "PS/2",
        "RJ-45",
        "Serial",
        "Stereo RCA"
    };
    private const string _serialNumberPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private List<_wire> _wires;
	private _complexWire _compWire;
    private string _serialNumber;
	private string _portType;
	private int _batteryCount;
	private static BombMemory _memory;

    private bool _isWireActive = true;
    private bool _bombDetonated = false;
    private bool _isMemoryActive = true;
    private bool IsMemoryDeactivated
    {
        get { return !_isMemoryActive; }
        set
        {
            if (_isMemoryActive != value)
            {
                _bombDetonated = value;
                if (!_isMemoryActive)
                {
                    OnMemoryDeactivated();
                }
            }
        }
    }
    private bool _isComplexWireActiveBackingField = true;
    private bool _isComplexWireActive
    {

        get => !((_compWire.Type == CompWireType.D) || !_isComplexWireActiveBackingField ||
                 !(_compWire.Type == CompWireType.S && _isLastDigitEven()) ||
                 !(_compWire.Type == CompWireType.P && _portType == "Parallel") ||
                 !(_compWire.Type == CompWireType.B) && _batteryCount >= 2);
        set
        {
            _isComplexWireActiveBackingField = value;
        }
    }
    private bool IsBombDetonated
    {
        get { return _bombDetonated; }
        set
        {
            if (_bombDetonated != value)
            {
                _bombDetonated = value;
                if (_bombDetonated)
                {
                    OnBombDetonated();
                }
            }
        }
    }

    private bool _isBombActive => _isWireActive || _isComplexWireActive || _isMemoryActive;

    private _complexWire ComplexWire
	{
		get => _compWire;
		set => _compWire = value;
	}
	private string SerialNumber
	{
		get => _serialNumber; 
	}
    private string PortType
	{
		get => _portType;
	}
    private int BatteryCount
	{
		get => _batteryCount; 
	}


    public Bomb()
    {
        _singleton = Helper.GetSingleton(_singleton, this);
        
        Random rd = new Random();

        StringBuilder sn = new StringBuilder(9);
        for (int i = 0; i < 9; i++)
        {
            sn.Append(_serialNumberPool[rd.Next(_serialNumberPool.Length)]);
        }
        _serialNumber = sn.ToString();

        int index = rd.Next(_portTypes.Length);
        _portType = _portTypes[index];

        _wires = new List<_wire>
        {
            new _wire((WireColor)rd.Next(Enum.GetValues(typeof(WireColor)).Length),1),
            new _wire((WireColor)rd.Next(Enum.GetValues(typeof(WireColor)).Length),1),
            new _wire((WireColor)rd.Next(Enum.GetValues(typeof(WireColor)).Length),1),
            new _wire((WireColor)rd.Next(Enum.GetValues(typeof(WireColor)).Length),1)
        };

		_memory = new BombMemory(this);
        _compWire = new _complexWire((CompWireType)rd.Next(Enum.GetValues(typeof(CompWireType)).Length), 1);
        _batteryCount = rd.Next(1,4);

    }
    
    internal void DisableBomb()
    {
        _singleton = null;
    }
    public void DisplayMemoryScreen()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;
        
        _memory.DisplayScreen();
    }
    public void PressMemoryButton(int buttonNum)
    {
        if (!Helper.IsSingleton(_singleton, this)) return;

        _memory.PressButton(buttonNum);
    }

    public void CheckBombModules()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;
        
        Console.WriteLine("Kablolar: " + (!_isWireActive ? "*deaktif*" : "aktif") + " durumda.");
        Console.WriteLine("Kompleks kablo: " + (_compWire.State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
        Console.WriteLine("Memory: " + (!_isMemoryActive ? "*deaktif*" : "aktif") + " durumda.");
    }
	public void CheckBombAttributes()
	{
        if (!Helper.IsSingleton(_singleton, this) && _singleton != null)return;
        
        
        if (!_bombDetonated)
        {
            Console.WriteLine("___________________________________");
            Console.WriteLine("Seri numarası: " + SerialNumber);
            Console.WriteLine("Port tipi: " + PortType);
            Console.WriteLine("Pil sayısı: " + BatteryCount);
            Console.WriteLine("___________________________________");
        }
        else
        {
            Console.WriteLine("Bomba çoktan patladı.");
        }
    }
	public void CheckWires()
	{
        if (!Helper.IsSingleton(_singleton, this)) return;
        
        
        if (!_bombDetonated)
        {
            Console.WriteLine("_____________________________________________________________________________________");
            Console.WriteLine("Kablo 0: " + _wires[0].Color + " renkli ve " + (_wires[0].State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
            Console.WriteLine("Kablo 1: " + _wires[1].Color + " renkli ve " + (_wires[1].State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
            Console.WriteLine("Kablo 2: " + _wires[2].Color + " renkli ve " + (_wires[2].State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
            Console.WriteLine("Kablo 3: " + _wires[3].Color + " renkli ve " + (_wires[3].State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
            Console.WriteLine("Kompleks kablo: " + _compWire.Type + " tipinde ve " + (_compWire.State == 0 ? "*kopuk*" : "bağlı") + " durumda.");
            Console.WriteLine("_____________________________________________________________________________________");
        }
        else
        {
            Console.WriteLine("Bomba çoktan patladı.");
        }
    }
    // public void PressAndReleaseBigButton()
    // {
    //     // _checkBombState();
    // }
    // public void PressAndHoldBigButton()
    // {
    //     // _checkBombState();
    // }
    public void CutWire(int wireIndex)
    {
        if (!Helper.IsSingleton(_singleton, this)) return;
        
        
        if (_isWireActive)
        {
            if (!_bombDetonated)
            {
                if (wireIndex < 0 || wireIndex >= _wires.Count)
                {
                    Console.WriteLine("Geçersiz kablo indeksi");
                    return;
                }

                _wires[wireIndex] = new _wire(_wires[wireIndex].Color, 0);
                Console.WriteLine(wireIndex + " indeksli kabloyu kestin.");

                int redWireCount = 0, blueWireCount = 0,
                    whiteWireCount = 0, greenWireCount = 0,
                    yellowWireCount = 0, blackWireCount = 0;

                foreach (var wire in _wires)
                {
                    if (wire.Color == WireColor.Kırmızı)
                        redWireCount++;
                    else if (wire.Color == WireColor.Mavi)
                        blueWireCount++;
                    else if (wire.Color == WireColor.Beyaz)
                        whiteWireCount++;
                    else if (wire.Color == WireColor.Yeşil)
                        greenWireCount++;
                    else if (wire.Color == WireColor.Sarı)
                        yellowWireCount++;
                    else if (wire.Color == WireColor.Siyah)
                        blackWireCount++;
                }


                int wireCondition;
                if (redWireCount > 1 && _isLastDigitOdd())
                    wireCondition = 0;
                else if (_wires[3].Color == WireColor.Sarı && redWireCount == 0)
                    wireCondition = 1;
                else if (blueWireCount == 1)
                    wireCondition = 2;
                else if (yellowWireCount > 1)
                    wireCondition = 3;
                else
                    wireCondition = 4;

                if ((wireCondition == 0 && _wires[wireIndex].Color == WireColor.Kırmızı) ||
                    (wireCondition == 1 && wireIndex == 0) ||
                    (wireCondition == 2 && wireIndex == 0) ||
                    (wireCondition == 3 && wireIndex == 3) ||
                    (wireCondition == 4) && wireIndex == 1)
                {
                    _isWireActive = false;
                    Console.WriteLine("**Kablolar başarıyla deaktive edildi.**");
                    _checkBombState();
                }
                else
                {
                    _bombDetonated = true;
                    OnBombDetonated();
                }
            }

            else if (!_isWireActive)
            {
                Console.WriteLine("Kablolar çoktan deaktive edildi.");
            }
            else
            {
                Console.WriteLine("Bomba çoktan patladı.");
            }
        }
        else
        {
            Console.WriteLine("Kablolar çoktan deaktive edildi.");
        }
    }
	public void CutComplexWire()
	{
        if (!Helper.IsSingleton(_singleton, this)) return;
        
        if (!_bombDetonated)
        {
            _compWire = new _complexWire(_compWire.Type, 0);
            Console.WriteLine("Kompleks kabloyu kestin.");

            if ((_compWire.Type == CompWireType.C) ||
                (_compWire.Type == CompWireType.S && _isLastDigitEven()) ||
                (_compWire.Type == CompWireType.P && _portType == "Parallel") ||
                (_compWire.Type == CompWireType.B) && _batteryCount >= 2)
            {
                _isComplexWireActive = false;
                Console.WriteLine("**Kompleks kablo başarıyla deaktive edildi.**");
                _checkBombState();
            }
            else
            {
                _bombDetonated = true;
                OnBombDetonated();
            }

        }
        else if (!_isComplexWireActive)
        {
            Console.WriteLine("Kompleks kablo çoktan deaktive edildi.");
        }
        else
        {
            Console.WriteLine("Bomba çoktan patladı.");
        }
        
    }

    public void GetWireManuel()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;

        Console.WriteLine("""
                          ----------------------------------------------------------------------------------------------------
                          Kablolar
                          Eğer birden fazla kırmızı kablo varsa ve seri numarasının son hanesi tekse, son kırmızı kabloyu kesin.
                          Aksi takdirde, son kablo sarıysa ve hiç kırmızı kablo yoksa, ilk kabloyu kesin.
                          Aksi takdirde, sadece bir mavi kablo varsa, ilk kabloyu kesin.
                          Aksi takdirde, birden fazla sarı kablo varsa, son kabloyu kesin.
                          Aksi takdirde, ikinci kabloyu kesin.
                          ----------------------------------------------------------------------------------------------------
                          """);
    }
    public void GetComplexWireManuel()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;

        Console.WriteLine("""
                          ----------------------------------------------------------------------------------------------------
                          Karmaşık Kablo
                          C Kabloyu kesin
                          D Kabloyu kesmeyin
                          S Seri numarasının son hanesi çiftse kabloyu kesin
                          P Bomba paralel bir port içeriyorsa kabloyu kesin
                          B Bomba iki veya daha fazla pil içeriyorsa kabloyu kesin
                          ----------------------------------------------------------------------------------------------------
                          """);
    }
    public void GetMemoryStageManuel()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;

        Console.WriteLine("""
                          ----------------------------------------------------------------------------------------------------
                          Aşama 1:
                          Eğer ekranda 1 varsa, birinci indexteki düğmeye basın.
                          Eğer ekranda 2 varsa, birinci indexteki düğmeye basın.
                          Eğer ekranda 3 varsa, ikinci indexteki düğmeye basın.
                          Eğer ekranda 4 varsa, üçüncü indexteki düğmeye basın.
                          
                          Aşama 2:
                          Eğer ekranda 1 varsa, "4" olarak etiketlenmiş düğmeye basın.
                          Eğer ekranda 2 varsa, birinci aşamada bastığınız pozisyondaki düğmeye basın.
                          Eğer ekranda 3 varsa, sıfırıncı indexteki düğmeye basın.
                          Eğer ekranda 4 varsa, birinci aşamada bastığınız indexteki düğmeye basın.
                          
                          Aşama 3:
                          Eğer ekranda 1 varsa, ikinci aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          Eğer ekranda 2 varsa, birinci aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          Eğer ekranda 3 varsa, ikinci indexteki düğmeye basın.
                          Eğer ekranda 4 varsa, "4" olarak etiketlenmiş düğmeye basın.
                          
                          Aşama 4:
                          Eğer ekranda 1 varsa, birinci aşamada bastığınız indexteki düğmeye basın.
                          Eğer ekranda 2 varsa, sıfırıncı indexteki düğmeye basın.
                          Eğer ekranda 3 varsa, ikinci aşamada bastığınız indexteki düğmeye basın.
                          Eğer ekranda 4 varsa, ikinci aşamada bastığınız indexteki düğmeye basın.
                          
                          Aşama 5:
                          Eğer ekranda 1 varsa, birinci aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          Eğer ekranda 2 varsa, ikinci aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          Eğer ekranda 3 varsa, dördüncü aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          Eğer ekranda 4 varsa, üçüncü aşamada bastığınız etiketle aynı etikete sahip düğmeye basın.
                          ----------------------------------------------------------------------------------------------------------
                          """);
    }

    
    public void GetAvailableFunctions()
    {
        if (!Helper.IsSingleton(_singleton, this)) return;

        Console.WriteLine("Bomba Classındaki Mevcut Fonksiyonlar:");
        for (int i = 0; i < _availableFunctions.Length; i++)
        {
            var function = _availableFunctions[i];
            Console.WriteLine(function);
        }
    }

    protected void _checkBombState()
    {
        if(!_isBombActive)
        {
            Console.WriteLine("***Bomba başarıyla imha edildi!***");
            Console.WriteLine("OYUNU KAZANDINIZ!!!!");
            Console.WriteLine("Tebrikler. Yeniden oynamak için Game'den oyunu tekrar başlatabilirsiniz");
            _gameEndedEvent?.Invoke();
        }
    }
    private bool _isLastDigitEven()
    {
        char lastChar = SerialNumber[SerialNumber.Length - 1];
        if (char.IsDigit(lastChar))
        {
            int lastDigit = lastChar - '0';
            return lastDigit % 2 == 0;
        }
        return false;
    }
    private bool _isLastDigitOdd()
    {
        char lastChar = SerialNumber[SerialNumber.Length - 1];
        if (char.IsDigit(lastChar))
        {
            int lastDigit = lastChar - '0';
            return lastDigit % 2 != 0;
        }
        return false;
    }
}
