using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] m_characterTextPros = null;

    [SerializeField]
    private Toggle m_timeBattleToggle = null;

    [SerializeField]
    private Button m_resetButton = null;

    private bool m_timeBattle = false;
    private List<Characters> m_characters = new List<Characters>();

    // Start is called before the first frame update
    void Start()
    {
        //create characters and add to list
        for (int i = 0; i< m_characterTextPros.Length; i++)
        {
            Characters character = GenerateCharacter(m_characterTextPros[i]);
            m_characters.Add(character);
        }

        //generate turn order
        SetBattleTurns();

        //Set time scale to 1
        Time.timeScale = 1.0f;

        //set time battle based on toggle setting
        m_timeBattle = m_timeBattleToggle.isOn;

        //register ui events for toggle and button clicks
        m_timeBattleToggle.onValueChanged.AddListener(delegate
        {
            ChangeTimeToggle();
        });

        m_resetButton.onClick.AddListener(delegate
        {
            ResetStats();
        });
    }

    //Update is called once per frame
    void Update()
    {
        if (m_timeBattle)
        {
            AddToTime();
        }
    }

    void AddToTime()
    {
        for (int i = 0; i < m_characters.Count; i++)
        {
            m_characters[i].timeBuiltUp += 1 * m_characters[i].speed;
            m_characters[i].SetBattleTimeText();
        }
    }

    void ChangeTimeToggle()
    {
        m_timeBattle = m_timeBattleToggle.isOn;

        ResetStats();
    }

    void SetBattleTurns()
    {
        //generate turn order
        m_characters = GenerateTurnOrder(new List<Characters>(m_characters), new List<Characters>());

        //set turn order information
        for (int i = 0; i < m_characters.Count; i++)
        {
            m_characters[i].battleTurn = i + 1;
            m_characters[i].SetBattleTurnText();
        }
    }

    void ResetStats()
    {
        for (int i = 0; i < m_characterTextPros.Length; i++)
        {
            Characters character = GenerateCharacter(m_characterTextPros[i]);
            character.SetBattleTimeText();
            m_characters[i] = character;
        }

        if (!m_timeBattle)
        {
            SetBattleTurns();
        }
    }

    Characters GenerateCharacter(TextMeshProUGUI textField)
    {
        Characters newCharacter = new Characters();
        newCharacter.speed = Random.Range(1, 8);
        newCharacter.timeBuiltUp = 0;
        newCharacter.characterText = textField;
        newCharacter.battleTurn = 0;

        return newCharacter;
    }

    List<Characters> GenerateTurnOrder(List<Characters> characters, List<Characters> orderedCharacters)
    {
        if(characters.Count < 1)
            return orderedCharacters;

        int highestSpeedChar = 0;
        for(int i=1; i<characters.Count; i++)
        {
            if (characters[i].speed > characters[highestSpeedChar].speed)
                highestSpeedChar = i;
        }

        orderedCharacters.Add(characters[highestSpeedChar]);
        characters.RemoveAt(highestSpeedChar);

        return GenerateTurnOrder(characters, orderedCharacters);
    }
}

class Characters
{
    public int speed;
    public int timeBuiltUp;
    public TextMeshProUGUI characterText;
    public int battleTurn;

    public void SetBattleTurnText()
    {
        characterText.SetText(battleTurn + "");
    }

    public void SetBattleTimeText()
    {
        characterText.SetText(timeBuiltUp + "");
    }
}
