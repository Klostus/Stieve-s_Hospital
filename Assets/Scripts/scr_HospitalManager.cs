using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class scr_HospitalManager : MonoBehaviour
{
    private Dictionary<int, string> patientsList; //список пациентов на лечении
    [HideInInspector]
    public int needToHealToday;
    [HideInInspector]
    public int needToHealYesterday; //для статистики за прошедший день
    [HideInInspector]
    public int wasHealToday;
    [HideInInspector]
    public int wasHealYesterday; //для статистики за прошедший день
    [HideInInspector]
    public bool death;

    private string[] patientsNames;
    [HideInInspector]
    public string newPatientName; //имя поступившего
    [HideInInspector]
    public int newPatientNumber; // порядковый номер поступившего
    [HideInInspector]
    public bool arrival;
    [HideInInspector]
    public GameObject chosenBed; // выделенная при поступлении койка

    private int bedsHospitalCount; //общее число койко-мест в больнице
    [HideInInspector]
    public int patientsHospitalCount;
    private int newPatientCondition; //степень тяжести состояния вновь поступившего пациента
    private GameObject[] greenBeds;
    private GameObject[] blueBeds;
    private GameObject[] redBeds;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    private scr_Economic economicScript;

    void Start()
    {
        defeatPanel.SetActive(false);
        victoryPanel.SetActive(false);
        // Находим все клоны кроватей на сцене по тэгам
        greenBeds = GameObject.FindGameObjectsWithTag("GreenBed") as GameObject[];
        blueBeds = GameObject.FindGameObjectsWithTag("BlueBed") as GameObject[];
        redBeds = GameObject.FindGameObjectsWithTag("RedBed") as GameObject[];
        bedsHospitalCount = greenBeds.Length + blueBeds.Length + redBeds.Length;
        patientsList = new Dictionary<int, string>();
        death = false;
        economicScript = GetComponent<scr_Economic>();
        NamesGenerator();
    }

    void Update()
    {
        ArrivalOfThePatient();
        if (death)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if(economicScript.currentCash >= economicScript.cashTarget)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    private void ArrivalOfThePatient()
    {
        if(patientsHospitalCount <= bedsHospitalCount)
        {
            if (arrival)
            {
                Random random = new Random();
                newPatientCondition = random.Next(1, 4);

                if (newPatientCondition == 1)
                    BedChoser(newPatientCondition, greenBeds);
                if (newPatientCondition == 2)
                    BedChoser(newPatientCondition, blueBeds);
                if (newPatientCondition == 3)
                    BedChoser(newPatientCondition, redBeds);

                foreach (var person in patientsList)
                {
                    Debug.Log($"key: {person.Key}   value: {person.Value}  summary: {patientsList.Count}");
                }

                needToHealToday += 1;
                arrival = false;
            }
        }
    }

    private void BedChoser(int condition, GameObject[] bedType)
    {
        bool stillSearching = true;
        for (int i = 0; i < bedType.Length && stillSearching; i++)
        {
            if (bedType[i].GetComponent<scr_BedStatus>().isNotEmpty == false)
            {
                bedType[i].GetComponent<scr_BedStatus>().patientCondition = condition;
                bedType[i].GetComponent<scr_BedStatus>().isNotEmpty = true;
                bedType[i].GetComponent<scr_BedStatus>().StateSwitcher(4);
                patientsHospitalCount += 1;
                newPatientNumber = patientsHospitalCount;
                Random randomName = new Random();
                newPatientName = patientsNames[randomName.Next(0, patientsNames.Length)];
                patientsList.Add(patientsHospitalCount, newPatientName);
                bedType[i].GetComponent<scr_BedStatus>().patientName = newPatientName;
                bedType[i].GetComponent<scr_BedStatus>().patientNumber = newPatientNumber;
                chosenBed = bedType[i];
                stillSearching = false;
            }
        }
    }

    private void NamesGenerator()
    { 
        patientsNames = new string []{
            "Вадим Павлович Жутяев",
            "Константин Юрьевич Ведешин", 
            "Валентин Аркадиевич Хазин", 
            "Пётр Евгеньевич Додукин",
            "Георгий Юриевич Чечнев",
            "Семён Юриевич Маркозов",
            "Аркадий Алексиевич Адоевцев",
"Виктор Евгеньевич Метлов",
"Герасим Борисович Решидов",
"Герасим Фёдорович Чалнов",
"Андрей Игоревич Костюхов",
"Дмитрий Игоревич Нясибуллин",
"Александр Леонидович Камышков",
"Анатолий Аркадиевич Кузовчиков",
"Михаил Валериевич Гаврюшкин",
"Кирилл Борисович Торкин",
"Денис Тимофеевич Куршонков",
"Николай Евгеньевич Бичуцкий",
"Геннадий Алексиевич Печерский",
"Анатолий Денисович Заколдаев",
"Павел Иванович Асямалов",
"Валерий Борисович Урютин",
"Олег Василиевич Жарковский",
"Ярослав Юриевич Бурусов",
"Максим Валериевич Пыричев",
"Федор Михаилович Чебуров",
"Валерий Адамович Стебелев",
"Леонид Тимофеевич Каравский",
"Виталий Антонович Мирошин",
"Герасим Борисович Силочкин",
"Юрий Павлович Мендюков",
"Владислав Юриевич Алеутский",
"Артур Виталиевич Потанов",
"Василий Антонович Купянский",
"Борис Юрьевич Абдулгадов",
"Степан Василиевич Жмулюкин",
"Вадим Юрьевич Лущиков",
"Антон Юриевич Валецкий",
"Роман Георгиевич Сосковский",
"Илья Викторович Евлешин",
"Константин Петрович Негодников",
"Степан Яковлевич Кибирев",
"Николай Аркадиевич Пензенский",
"Геннадий Борисович Хлянов",
"Николай Петрович Мадышев",
"Леонид Максимович Дмитряков",
"Вадим Адамович Абашин",
"Максим Георгиевич Камалин",
"Виталий Борисович Анаников",
"Никита Николаевич Карачалов",
"Василий Олегович Панкрашин",
"Иван Фёдорович Обезьянин",
"Антон Иванович Конушкин",
"Яков Борисович Багзин",
"Степан Сергеевич Буслеев",
"Артём Вадимович Айдерханов",
"Евгений Виталиевич Пряслов",
"Борис Андреевич Кончихин",
"Тарас Василиевич Мотовилов",
"Федор Андреевич Тузулков",
"Алиса Егоровна Томкина",
"Ирина Михайловна Сикерская",
"Раиса Петровна Колобамова",
"Анна Петровна Пупынина",
"Зинаида Вадимовна Белоконова",
"Александра Ивановна Калуцкая",
"Ольга Никитична Лягушева",
"Галина Игоревна Дрожинова",
"Виктория Сергеевна Грозина",
"Кристина Сергеевна Капшина",
"Александра Эдуардовна Таврина",
"Маргарита Яковлевна Биочинская",
"Виолетта Аркадьевна Загодская",
"Кристина Федоровна Сикирева",
"Евгения Олеговна Горшнева",
"Алиса Егоровна Синдяшкина",
"Виолетта Игоревна Жагланова",
"Ксения Федоровна Шемялинина",
"Юлия Игоревна Владысева",
"Жанна Михайловна Ильюшина",
"Инна Василевна Табакина",
"Галина Викторовна Ивешева",
"Клара Яковлевна Катжанова",
"Катерина Ивановна Гибизова",
"Тамара Богдановна Батяйкина",
"Елена Валерьевна Шенберева",
"Катерина Федоровна Бригадова",
"Галина Эдуардовна Сочинская",
"Елена Викторовна Осьянова",
"Любовь Сергеевна Куравцева",
"Оксана Викторовна Ашихнина",
"Виолетта Яковлевна Харабурова",
"Инна Василевна Анчурова",
"Ольга Сергеевна Брылунова",
"Виолетта Егоровна Жечкова",
"Кира Алексеевна Быдлова",
"Любовь Максимовна Кузмицкая",
"Карина Вадимовна Вялиуллина",
"Елена Николаевна Осычина",
"Оксана Андреевна Каленикова"};
    }
}
