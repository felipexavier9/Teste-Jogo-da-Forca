using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

using TMPro;

public class GameManager : MonoBehaviour
{
    public TMP_InputField letterInput;

    public string[] wordList;
    public List<string> solvedList = new List<string>();
    public string[] unsolvedWord;

    public List<TMP_Text> letterHolderList = new List<TMP_Text>();
    public GameObject letterPrefab;
    public Transform letterHolder;

    int mistakes;
    public int maxMistakes = 5;
    public TMP_Text mistakeCounter;
    public List<TMP_Text> mistakeList = new List<TMP_Text>();
    public Transform mistakeHolder;
    public GameObject mistakePrefab;
    bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Initialize()
    {
        //Escolhe uma palavra da lista de palavras
        int index = Random.Range(0, wordList.Length);
        string tempWord = wordList[index];

        //Separa as letras em uma lista que será a palavra oculta
        unsolvedWord = new string[tempWord.Length];
        for( int i = 0; i < tempWord.Length; i++)
        {
            solvedList.Add(tempWord[i].ToString().ToUpper());
        }

        //Desenha a interface gráfica
        for( int i = 0; i < solvedList.Count; i++)
        {
            GameObject tempObj = Instantiate(letterPrefab,letterHolder, false);
            letterHolderList.Add(tempObj.GetComponent<TMP_Text>());
        }

    }

    //Método que recebe e trata a entrada das letras
    public void InputButton()
    {
        //Se o jogo terminou ignora a entrada
        if(gameOver)
        {
            return;
        }

        string inputLetter = letterInput.text;

        //Se a entrada for um espaço vazio ignora entrada
        if (inputLetter == "" || inputLetter == " ")
        {
            return;
        }

        //Verifica a letra
        CheckLetter(inputLetter.ToUpper());
        
        //Retorna a entrada para o estado inicial
        letterInput.text = "";
    }

    //Método que verifica a letra recebida
    void CheckLetter( string letter)
    {
        bool hasFoundLetter = false;

        //Itera sobre a palavra oculta e onde a letra esteja presente a torna vizível para o jogador
        //e marca 'true' para a variável de letra encontrada
        for (int i = 0; i < solvedList.Count; i++)
        {
            if ( solvedList[i] == letter)
            {
                letterHolderList[i].text = letter.ToString();
                unsolvedWord[i] = letter;
                hasFoundLetter = true;
            }
        }

        //Caso a letra não esteja presente, aumenta o contador de erros, adiciona a letra à lista
        //de letras erradas e verifica se o jogador passou do limite de erros
        if(!hasFoundLetter)
        {
            mistakes++;
            mistakeCounter.text = mistakes.ToString();
            GameObject tempObj = Instantiate(mistakePrefab, mistakeHolder, false);
            tempObj.GetComponent<TMP_Text>().text = letter;
            mistakeList.Add(tempObj.GetComponent<TMP_Text>());

            if( mistakes == maxMistakes)
            {
                Debug.Log("Game is over!");
                gameOver = true;
            }
        }

        //Verifica se o jogador venceu
        Debug.Log("player won? "+CheckIfWon());
    }

    //Verifica se o jogador venceu o jogo
    bool CheckIfWon()
    {
        //Itera sobre as letras reveladas, se formarem a palavra oculta retorna 'true' 
        //ou 'false' caso contrário
        for (int i = 0; i < unsolvedWord.Length; i++)
        {
            if (unsolvedWord[i] != solvedList[i])
            {
                return false;
            }
        }
        return true;
    }
}
